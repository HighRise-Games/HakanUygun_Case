using System.Collections.Generic;
using System.Linq;

public static class AnalyticsManager
{
    private static bool _isAnalyticsEnabled = true;

    public static void SetAnalyticsActive(bool status)
    {
        _isAnalyticsEnabled = status;
    }

    private static int _lastPlayedLevelIndex;

    public static void OnLevelStart(Dictionary<string, object> extraParams = null)
    {
        if (!_isAnalyticsEnabled)
            return;

        var lvlIndex = DataManager.CurrentLevelIndex;
        var lvlId = GameManager.Instance.levelManager.currentLevel.levelId;

        var startType = _lastPlayedLevelIndex == lvlIndex ? StartType.Retry : StartType.First;

        _lastPlayedLevelIndex = lvlIndex;

#if RUBY_FRAMEWORK
        RubyGames.Framework.RubyFramework.OnGameStarted(lvlIndex, lvlId, (RubyGames.Framework.Common.StartType) startType, extraParams);
#endif
        
        LogManager.Log($"AnalyticsHelper OnLevelStarted - Level index:{lvlIndex}, Level id:{lvlId} " +
                       $"Start type:{startType}");
    }

    public static void OnLevelFinish(EndType endType, Dictionary<string, object> extraParams = null)
    {
        if (!_isAnalyticsEnabled)
            return;

        var lvlIndex = DataManager.CurrentLevelIndex;
        var lvlId = GameManager.Instance.levelManager.currentLevel.levelId;

#if RUBY_FRAMEWORK
        RubyGames.Framework.RubyFramework.OnGamePlayed(lvlIndex, lvlId, (RubyGames.Framework.Common.EndType) endType, extraParams);
#endif
        
        LogManager.Log($"AnalyticsHelper OnLevelEnded - Level index:{lvlIndex}, Level id:{lvlId} " +
                       $"End type:{endType}, extra params:{extraParams}");
    }
    
    public static void OnProgress(string eventName, Dictionary<string, object> parameters = null)
    {
#if RUBY_FRAMEWORK
        if (parameters is not null)
            RubyGames.Framework.RubyFramework.TrackEvent(eventName, parameters);
        else
            RubyGames.Framework.RubyFramework.TrackEvent(eventName);
#endif
        var parametersString = "Null";

        if (parameters is not null)
            parametersString = parameters.Aggregate("",
                (current, param) => current + $"{System.Environment.NewLine} Key : {param.Key}, Value : {param.Value}");

        LogManager.Log($"AnalyticsHelper OnProgress - Event Name :{eventName} , Parameters :{parametersString}");
    }
}

public enum StartType
{
    First,
    Retry,
    Continue,
}

public enum EndType
{
    Success,
    Fail,
    Quit,
    Skip,
}