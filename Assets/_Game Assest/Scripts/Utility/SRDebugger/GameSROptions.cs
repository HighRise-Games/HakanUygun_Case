#if !DISABLE_SRDEBUGGER
using JetBrains.Annotations;
using System.ComponentModel;

#if RUBY_FRAMEWORK
using RubyGames;
using RubyGames.Framework;
using RubyGames.Framework.Config;
using RubyGames.Framework.Settings.Extensions;
#endif
using SRDebugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameSROptions : INotifyPropertyChanged
{
    private static GameSROptions _instance;
    public static GameSROptions Instance => _instance ??= new GameSROptions();

    [Category("Game")]
    public float TimeScale
    {
        get => Time.timeScale;
        set => Time.timeScale = value;
    }

    [Category("Save")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared successfully!");
    }

    [Category("Economy")]
    public void AddCurrency()
    {
        DataManager.Currency += 1000;
    }

    [Category("Economy")]
    public int Currency
    {
        get => DataManager.Currency;
        set => DataManager.Currency = value;
    }

    [Category("Levels")]
    public int Level
    {
        get => DataManager.CurrentLevelIndex;
        set => DataManager.CurrentLevelIndex = value;
    }

    [Category("Levels")]
    public void ReloadScene()
    {
        LevelManager.LoadNextLevel();
    }

    [Category("Levels")]
    public void FailLevel()
    {
        GameManager.Instance.LevelFinish(false);
    }

    [Category("Levels")]
    public void CompleteLevel()
    {
        GameManager.Instance.LevelFinish(true);
    }

#if RUBY_FRAMEWORK_ADDON
    private static DynamicOptionContainer remoteConfigContainer = new DynamicOptionContainer();

    [Category("RemoteConfig")]
    public void AddRemoteConfigSettings()
    {
        if (remoteConfigContainer?.Options.Count > 0)
        {
            Debug.Log($"Remote config options already enabled!");
            return;
        }

        if (remoteConfigContainer != null)
        {
            SRDebug.Instance.RemoveOptionContainer(remoteConfigContainer);
        }

        remoteConfigContainer = new DynamicOptionContainer();
        SRDebug.Instance.AddOptionContainer(remoteConfigContainer);

        var configValues = RubyFramework.Settings.GetConfigValues();

        foreach (var configValue in configValues)
        {
            OptionDefinition option = null;

            switch (configValue.Value)
            {
                case string isString:
                    option = CreateOptionForType<string>(configValue.Key);
                    break;
                case char isChar:
                    option = CreateOptionForType<Char>(configValue.Key);
                    break;
                case int isInt:
                    option = CreateOptionForType<int>(configValue.Key);
                    break;
                case double isDouble:
                    option = CreateOptionForType<double>(configValue.Key);
                    break;
                case float isFloat:
                    option = CreateOptionForType<float>(configValue.Key);
                    break;
                case decimal isDecimal:
                    option = CreateOptionForType<decimal>(configValue.Key);
                    break;
                case bool isBool:
                    option = CreateOptionForType<bool>(configValue.Key);
                    break;
                default:
                    Debug.Log($"Creating option for {configValue.Key} has failed!");
                    break;
            }

            if (option != null)
            {
                remoteConfigContainer.AddOption(option);
            }
        }

        Debug.Log($"Remote config options enabled!");
    }

    private OptionDefinition CreateOptionForType<T>(string overrideKey)
    {
        var option = OptionDefinition.Create<T>(overrideKey,
            () => (T)RubyFramework.Settings.GetConfigValue(overrideKey),
            (newValue) =>
            {
                RubyFramework.Settings.AddOverrideConfig(overrideKey, newValue.ToString());
                RubyFramework.Settings.SetConfigValueRuby(overrideKey, new RubyConfigValue(newValue.ToString()));
                // RubyFramework.Settings.AddOverrideConfig(overrideKey, RubyFramework.Settings.GetConfigValue(overrideKey).ToString());
                // RubyFramework.Settings.SetOverrideConfigValue(overrideKey, new RubyConfigValue(newValue.ToString()));
            },
            "RemoteConfigOptions");
        return option;
    }

    [Category("RemoteConfig")]
    public void RemoveRemoteConfigSettings()
    {
        if (remoteConfigContainer != null)
        {
            RubyFramework.Settings.DebugOverrideValues.Clear();
            SRDebug.Instance.RemoveOptionContainer(remoteConfigContainer);
            RubyFramework.Settings.SetConfigValuesFirebase();
            remoteConfigContainer = null;
            Debug.Log($"Remote config options disabled!");
        }
    }

    private void AddOverrideToRubyFrameworkSettings(string overrideKey, string overrideValue)
    {
        var overrideList = RubyFramework.Settings.DebugOverrideValues;
        if (overrideList.Any(x => x.Key == overrideKey))
        {
            var existingOverride = overrideList.FirstOrDefault(x => x.Key == overrideKey);
            existingOverride.Key = overrideKey;
            existingOverride.Value = overrideValue;
        }
        else
        {
            overrideList.Add(new ConfigDetails() { Key = overrideKey, Value = overrideValue });
        }
    }

    private void RemoveOverrideFromRubyFrameworkSettings(string overrideKey)
    {
        var overrideList = RubyFramework.Settings.DebugOverrideValues;
        if (overrideList.Any(x => x.Key == overrideKey))
        {
            overrideList.RemoveAll(config => config.Key == overrideKey);
        }
    }
#endif

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
#endif