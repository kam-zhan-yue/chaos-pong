using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public class CustomPostProcessRenderFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader bloomShader;
    [SerializeField] private Shader compositeShader;

    private Material _bloomMaterial;
    private Material _compositeMaterial;
    
    private CustomPostProcessPass _customPass;
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_customPass);
    }
    
    public override void Create()
    {
        _bloomMaterial = CoreUtils.CreateEngineMaterial(bloomShader);
        _compositeMaterial = CoreUtils.CreateEngineMaterial(compositeShader);
        _customPass = new CustomPostProcessPass(_bloomMaterial, _compositeMaterial);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_bloomMaterial);
        CoreUtils.Destroy(_compositeMaterial);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            _customPass.ConfigureInput(ScriptableRenderPassInput.Depth);
            _customPass.ConfigureInput(ScriptableRenderPassInput.Color);
            _customPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }
    }
}
