using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class GameBuilder
{
    [MenuItem("Build/Build macOS")]
    public static void PerformMacOSBuild()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Prototype.unity" };
        buildPlayerOptions.locationPathName = "build/macOS/ChaosPong.app";
        buildPlayerOptions.target = BuildTarget.StandaloneOSX;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
            Debug.Log($"Build Succeeded: {summary.totalSize} bytes");
        else if (summary.result == BuildResult.Failed)
            Debug.Log("Build Failed");
    }
    
    [MenuItem("Build/Build WebGL")]
    public static void PerformWebGLBuild()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Prototype.unity" };
        buildPlayerOptions.locationPathName = "build/WebGL";
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
            Debug.Log($"Build Succeeded: {summary.totalSize} bytes");
        else if (summary.result == BuildResult.Failed)
            Debug.Log("Build Failed");
    }

    
    [MenuItem("Build/Build Windows 64")]
    public static void PerformWindows64Build()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Prototype.unity" };
        buildPlayerOptions.locationPathName = "build/Windows";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
            Debug.Log($"Build Succeeded: {summary.totalSize} bytes");
        else if (summary.result == BuildResult.Failed)
            Debug.Log("Build Failed");
    }
}