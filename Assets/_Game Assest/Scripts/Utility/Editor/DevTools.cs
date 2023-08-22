using System;
using System.IO;
using Facebook.Unity.Settings;
using RubyGames.Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    [InitializeOnLoad]
    public abstract class DevTools
    {
        //To create a hotkey you can use the following special characters:
        //% (ctrl on Windows and Linux, cmd on macOS),
        //^ (ctrl on Windows, Linux, and macOS),
        //# (shift), & (alt).
        //If no special modifier key combinations are required the key can be given after an underscore.

        [MenuItem("Tools/Ruby Framework/Options " + "&o", false, 100)]
        public static void ShowRubyFrameworkOptionsWindow()
        {
            RubyFrameworkOptionsEditorWindow.ShowWindow();
        }

        [MenuItem("Tools/Ruby Framework/Create or Open Settings " + "&s", false, 100)]
        public static void CreateOrOpenSettings()
        {
            var facebookSettings = Resources.Load(FacebookSettings.FacebookSettingsPath, typeof(FacebookSettings));
            if (facebookSettings == null)
            {
                if (!Directory.Exists(Application.dataPath + "/FacebookSDK/SDK/Resources"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/FacebookSDK/SDK/Resources");
                }

                var fbAsset = ScriptableObject.CreateInstance<FacebookSettings>();
                AssetDatabase.CreateAsset(fbAsset,
                    "Assets/" + FacebookSettings.FacebookSettingsPath + "/" +
                    FacebookSettings.FacebookSettingsAssetName + FacebookSettings.FacebookSettingsAssetExtension);
                AssetDatabase.Refresh();

                AssetDatabase.SaveAssets();
                Debug.LogWarning("Ruby Analytics Facebook: Settings file didn't exist and was created");
            }

            var settings = UnityEngine.Resources.Load<RubyFrameworkSettings>("RubyFrameworkSettings");
            if (settings != null)
            {
                UnityEngine.Debug.LogWarning("RubyFrameworkSettings is already created.");
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = settings;
                return;
            }

            var asset = UnityEngine.ScriptableObject.CreateInstance<RubyFrameworkSettings>();

            var path = "Assets/Resources/";
            var resourcesDirectoryPath = Path.Combine(Application.dataPath, "Resources");
            if (!Directory.Exists(resourcesDirectoryPath))
            {
                Debug.LogWarning("Resources folder could not be found in Assets directory.");
                Debug.LogWarning($"Creating Resources folder in: \"{resourcesDirectoryPath}\"");
                Directory.CreateDirectory(resourcesDirectoryPath);
            }

            var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "RubyFrameworkSettings" + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        [MenuItem("Tools/Dev Tools/Clear All PlayerPrefs " + "&c", false, 100)]
        public static void ClearAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/Dev Tools/Next Level " + "&n", false, 100)]
        public static void NextLevel()
        {
            if (!Application.isPlaying)
                return;

            DataManager.IsTutorial = false;
            DataManager.CurrentLevelIndex++;
            LevelManager.ReloadScene();
        }
        
        [MenuItem("Tools/Dev Tools/Previous Level " + "&#n", false, 100)]
        public static void PreviousLevel()
        {
            if (!Application.isPlaying)
                return;

            DataManager.CurrentLevelIndex = DataManager.CurrentLevelIndex > 0 ? DataManager.CurrentLevelIndex - 1 : 0;
            DataManager.IsTutorial = DataManager.CurrentLevelIndex == 0;
            LevelManager.ReloadScene();
        }

        [MenuItem("Tools/Dev Tools/Add Money " + "&m", false, 100)]
        public static void AddMoney()
        {
            DataManager.Currency += 10000;
        }

        [MenuItem("Tools/Dev Tools/Clear Money " + "&#m", false, 100)]
        public static void ClearMoney()
        {
            DataManager.Currency = 1;
        }


        [MenuItem("Tools/Dev Tools/Reload Domain " + "&r", false, 100)]
        public static void ReloadDomain()
        {
            EditorUtility.RequestScriptReload();
        }

        private static bool _isPlayModeRegistered = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticValues()
        {
            if (_isPlayModeRegistered)
                return;

            EditorApplication.playModeStateChanged += OnPlayModeChange;
            _isPlayModeRegistered = true;
        }

        private static void OnPlayModeChange(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingPlayMode:
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        static DevTools()
        {
            EditorApplication.update += ControlTimeScale;
        }

        private static void ControlTimeScale()
        {
            Time.timeScale = Application.isPlaying switch
            {
                true when Input.GetKey(KeyCode.S) => Input.GetKey(KeyCode.LeftShift) ? 7.5f : 2.5f,
                true when Input.GetKey(KeyCode.D) => Input.GetKey(KeyCode.LeftShift) ? .1f : .25f,
                _ => 1f
            };
        }
    }
}