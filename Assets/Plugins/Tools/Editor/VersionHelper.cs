using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class VersionHelper : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    [MenuItem("Tools/Build/Increase Major Version", false, 100)]
    private static void IncreaseMajor()
    {
        IncrementVersion(new[] { 1, 0, 0 });
    }

    [MenuItem("Tools/Build/Increase Minor Version", false, 100)]
    private static void IncreaseMinor()
    {
        IncrementVersion(new[] { 0, 1, 0 });
    }

    [MenuItem("Tools/Build/Increase Build Version", false, 100)]
    private static void IncreaseBuild()
    {
        IncrementVersion(new[] { 0, 0, 1 });
    }

    [MenuItem("Tools/Build/Increase Platforms Version (Android and iOS) " + "&v", false, 100)]
    private static void IncreasePlatformsVersion()
    {
        PlayerSettings.Android.bundleVersionCode += 1;
        PlayerSettings.iOS.buildNumber = (int.Parse(PlayerSettings.iOS.buildNumber) + 1).ToString();
        Debug.Log($"New Android bundle version code: {PlayerSettings.Android.bundleVersionCode}");
        Debug.Log($"New iOS build number: {PlayerSettings.iOS.buildNumber}");
    }

    static void IncrementVersion(int[] version)
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');

        for (int i = lines.Length - 1; i >= 0; i--)
        {
            bool isNumber = int.TryParse(lines[i], out int numberValue);

            if (isNumber && version.Length - 1 >= i)
            {
                if (i > 0 && version[i] + numberValue > 9)
                {
                    version[i - 1]++;

                    version[i] = 0;
                }
                else
                {
                    version[i] += numberValue;
                }
            }
        }

        PlayerSettings.bundleVersion = $"{version[0]}.{version[1]}.{version[2]}";
        Debug.Log($"New bundle version: {version[0]}.{version[1]}.{version[2]}");
    }

    public void OnPreprocessBuild(BuildReport report)
    {
#if !DEV_MODE && !DISABLE_SRDEBUGGER
        throw new BuildFailedException("Missing Configuration for SR Debugger !!!");
#endif

        var shouldIncrement = EditorUtility.DisplayDialog(
            title: "Increment Platforms Version?",
            message: $@"
                Current version: {PlayerSettings.bundleVersion}
                Android bundle version: {PlayerSettings.Android.bundleVersionCode}
                iOS build number: {PlayerSettings.iOS.buildNumber}",
            ok: "Yes",
            cancel: "No"
        );

        if (shouldIncrement) IncreasePlatformsVersion();
    }
}