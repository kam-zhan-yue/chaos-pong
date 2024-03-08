using System;
using System.Collections;
using System.Collections.Generic;
using Signals;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public class CustomPostProcessPass : ScriptableRenderPass
{
    private Material m_BloomMaterial;
    private Material m_compositeMaterial;
    private RenderTextureDescriptor m_Descriptor;
    private RTHandle m_CameraColorTarget;
    private RTHandle m_CameraDepthTarget;

    private const int k_MaxPyramidSize = 6;
    private int[] _BloomMipUp;
    private int[] _BloomMipDown;
    //an RTHandle is a RenderTexture that scales automatically with camera size
    private RTHandle[] m_BloomMipUp;
    private RTHandle[] m_BloomMipDown;
    private GraphicsFormat hdrFormat;

    private BenDayBloomEffectComponent m_BloomEffect;

    public CustomPostProcessPass(Material mBloomMaterial, Material mCompositeMaterial)
    {
        m_BloomMaterial = mBloomMaterial;
        m_compositeMaterial = mCompositeMaterial;

        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        _BloomMipUp = new int[k_MaxPyramidSize];
        _BloomMipDown = new int[k_MaxPyramidSize];
        m_BloomMipUp = new RTHandle[k_MaxPyramidSize];
        m_BloomMipDown = new RTHandle[k_MaxPyramidSize];

        for (int i = 0; i < k_MaxPyramidSize; ++i)
        {
            _BloomMipUp[i] = Shader.PropertyToID("_BloomMipUp" + i);
            _BloomMipDown[i] = Shader.PropertyToID("_BloomMipDown" + i);
            m_BloomMipUp[i] = RTHandles.Alloc(_BloomMipUp[i], name: "_BloomMipUp" + i);
            m_BloomMipDown[i] = RTHandles.Alloc(_BloomMipDown[i], name: "_BloomMipUp" + i);
        }

        const FormatUsage usage = FormatUsage.Linear | FormatUsage.Render;
        if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, usage))
        {
            hdrFormat = GraphicsFormat.B10G11R11_UFloatPack32;
        }
        else
        {
            hdrFormat = QualitySettings.activeColorSpace == ColorSpace.Linear
                ? GraphicsFormat.R8G8B8A8_SRGB
                : GraphicsFormat.R8G8B8A8_SNorm;
        }
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        VolumeStack stack = VolumeManager.instance.stack;
        m_BloomEffect = stack.GetComponent<BenDayBloomEffectComponent>();
        
        //A command buffer is basically a list of rendering tasks that we want to perform
        CommandBuffer cmd = CommandBufferPool.Get();
        
        //Allows us to visualise how it looks in the frame debugger
        using (new ProfilingScope(cmd, new ProfilingSampler("Custom Post Process Effects")))
        {
            //Do the bloom pass here first
            SetupBloom(cmd, m_CameraColorTarget);
            
            //Setup composite values
            m_compositeMaterial.SetFloat("_Cutoff",  m_BloomEffect.dotsCutoff.value);
            m_compositeMaterial.SetFloat("_Density",  m_BloomEffect.dotsDensity.value);
            m_compositeMaterial.SetVector("_Direction",  m_BloomEffect.scrollDirection.value);
            
            Blitter.BlitCameraTexture(cmd, m_CameraColorTarget, m_CameraColorTarget, m_compositeMaterial, 0);
        }

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        m_Descriptor = renderingData.cameraData.cameraTargetDescriptor;
    }

    public void SetTarget(RTHandle cameraColorTargetHandle, RTHandle cameraDepthTargetHandle)
    {
        m_CameraColorTarget = cameraColorTargetHandle;
        m_CameraDepthTarget = cameraDepthTargetHandle;
    }

    private void SetupBloom(CommandBuffer cmd, RTHandle source)
    {
        // Start at half-res
        int downres = 1;
        int tw = m_Descriptor.width >> downres;
        int th = m_Descriptor.height >> downres;
        // Determine the iteration count
        int maxSize = Math.Max(tw, th);
        int iterations = Mathf.FloorToInt (Mathf.Log(maxSize, 2f) - 1);
        int mipCount = Mathf.Clamp(iterations, 1, m_BloomEffect. maxIterations.value);
        // Pre-filtering parameters
        float clamp = m_BloomEffect.clamp. value;
        float threshold = Mathf.GammaToLinearSpace(m_BloomEffect.threshold.value);
        float thresholdKnee = threshold * 0.5f; // Hardcoded soft knee
        // Material setup
        float scatter = Mathf.Lerp(0.05f, 0.95f, m_BloomEffect.scatter. value);
        var bloomMaterial = m_BloomMaterial;
        bloomMaterial.SetVector("_Params", new Vector4(scatter, clamp, threshold, thresholdKnee));
        //Prefilter
        var desc = GetCompatibleDescriptor(tw, th, hdrFormat);
        for (int i = 0; i < mipCount; i++)
        {
            RenderingUtils.ReAllocateIfNeeded(ref m_BloomMipUp[i],desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name:m_BloomMipUp[i].name);
            RenderingUtils.ReAllocateIfNeeded(ref m_BloomMipDown[i],desc, FilterMode.Bilinear,TextureWrapMode.Clamp, name:m_BloomMipDown[i].name);
            desc.width = Mathf.Max(1, desc.width >> 1);
            desc.height = Mathf.Max(1, desc. height>> 1);
        }

        Blitter.BlitCameraTexture(cmd, source, m_BloomMipDown[0], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 0);
        
        //Down sample - gaussian pyramid
        var lastDown = m_BloomMipDown[0];
        for (int i = 1; i < mipCount; ++i)
        {
            //Classic two pass gaussian blur - use mipUp as a temporary target
            //First pass does 2x downsampling + 9-tap gaussian
            //Second pass does 9-tap gaussian using a 5-tap filter + bilinear filtering
            Blitter.BlitCameraTexture(cmd, lastDown, m_BloomMipUp[i], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 1);
            Blitter.BlitCameraTexture(cmd, m_BloomMipUp[i], m_BloomMipDown[i], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 2);

            lastDown = m_BloomMipDown[i];
        }
        
        // Up Sample (bilinear by default, HQ filtering does bicubic instead
        for (int i = mipCount - 2; i >= 0; --i)
        {
            var lowMip = (i == mipCount - 2) ? m_BloomMipDown[i + 1] : m_BloomMipUp[i + 1];
            var highMip = m_BloomMipDown[i];
            var dst = m_BloomMipUp[i];
            
            cmd.SetGlobalTexture("_SourceTexLowMip", lowMip);
            Blitter.BlitCameraTexture(cmd, highMip, dst, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store,
                bloomMaterial, 3);
        }
        
        cmd.SetGlobalTexture("_BloomTexture", m_BloomMipUp[0]);
        cmd.SetGlobalFloat("_BloomIntensity", m_BloomEffect.intensity.value);
    }

    private RenderTextureDescriptor GetCompatibleDescriptor() =>
        GetCompatibleDescriptor(m_Descriptor.width, m_Descriptor.height, m_Descriptor.graphicsFormat);

    private RenderTextureDescriptor GetCompatibleDescriptor(int width, int height, GraphicsFormat format,
        DepthBits depthBufferBits = DepthBits.None)
        => GetCompatibleDescriptor(m_Descriptor, width, height, format, depthBufferBits);

    internal static RenderTextureDescriptor GetCompatibleDescriptor(RenderTextureDescriptor desc, int width, int height,
        GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
    {
        desc.depthBufferBits = (int)depthBufferBits;
        desc.msaaSamples = 1;
        desc.width = width;
        desc.height = height;
        desc.graphicsFormat = format;
        return desc;
    }
}
