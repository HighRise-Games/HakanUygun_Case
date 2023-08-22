#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace Plugins.Tools.Editor
{
    public static class PostProcessBuildSettings
    {
        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuildProject)
        {
            if (buildTarget != BuildTarget.iOS) return;
            var pbxPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
            var pbxProject = new PBXProject();
            pbxProject.ReadFromFile(pbxPath);

            var target = pbxProject.GetUnityFrameworkTargetGuid();
            //Disabling ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES Unity Framework target
            pbxProject.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            //Set App Uses Non-Exempt Encryption to No
            pbxProject.SetBuildProperty(target, "ITSAppUsesNonExemptEncryption", "NO");
            
            //Disabling Bitcode on all targets
            //Main
            pbxProject.SetBuildProperty(pbxProject.GetUnityMainTargetGuid(), "ENABLE_BITCODE", "NO");
            //Unity Tests
            pbxProject.SetBuildProperty(pbxProject.TargetGuidByName(PBXProject.GetUnityTestTargetName()), "ENABLE_BITCODE", "NO");
            //Unity Framework
            pbxProject.SetBuildProperty(pbxProject.GetUnityFrameworkTargetGuid(), "ENABLE_BITCODE", "NO");

            pbxProject.WriteToFile(pbxPath);

            var guid = pbxProject.GetUnityMainTargetGuid();
            var idArray = Application.identifier.Split('.');
            var entitlementsPath = $"Unity-iPhone/{idArray[^1]}.entitlements";
            var capManager = new ProjectCapabilityManager(pbxPath, entitlementsPath, null, guid);

            // Add Push Notifications Capability to Project
            capManager.AddPushNotifications(false);

            capManager.WriteToFile();
        }
    }
}
#endif