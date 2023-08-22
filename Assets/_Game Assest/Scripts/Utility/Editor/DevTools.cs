using System;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    public abstract class DevTools
    {
        //To create a hotkey you can use the following special characters:
        //% (ctrl on Windows and Linux, cmd on macOS),
        //^ (ctrl on Windows, Linux, and macOS),
        //# (shift), & (alt).
        //If no special modifier key combinations are required the key can be given after an underscore.

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

            DataManager.CurrentLevelIndex++;
            LevelManager.LoadNextLevel();
        }

        [MenuItem("Tools/Dev Tools/Add Money " + "&m", false, 100)]
        public static void AddMoney()
        {
            DataManager.Currency += 10000;
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


        //
        // [SerializeField] public static GameObject rubyFrameworkInjector; 
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        // public static void Execute()
        // {
        //     Object.Instantiate(rubyFrameworkInjector);
        // }
    }
}