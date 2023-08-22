using System;
using System.Collections;
using UnityEngine;

public static class AdManager
{
    private static float _lastInterstitialRequestTime;

    public static void ShowInterstitial(string placement)
    {
        if (Time.time < _lastInterstitialRequestTime)
        {
            return;
        }

        // if (DataManager.IsTutorial || TutorialController.NextInterstitialTime > Time.realtimeSinceStartup)
        // {
        //     LogManager.Log($"Player In Tutorial {DataManager.IsTutorial} or Tutorial time " +
        //                    $"{TutorialController.NextInterstitialTime} not meet {Time.realtimeSinceStartup} ");
        //     return;
        // }

        placement += "_interstitial";

#if RUBY_FRAMEWORK_ADS
        RubyGames.Framework.RubyFramework.ShowInterstitial(placement);
#endif

        _lastInterstitialRequestTime = Time.time + 1;
    }

    public static void ShowRewarded(string placement, Action<bool> onComplete)
    {
        placement += "_rewarded";

#if RUBY_FRAMEWORK_ADS
        RubyGames.Framework.RubyFramework.ShowRewardedVideo(placement, onComplete);
#else
        onComplete?.Invoke(true);
#endif
    }

    public static void ActivateBanner()
    {
        GameManager.Instance.StartCoroutine(BannerActivationCheck());
    }

    private static IEnumerator BannerActivationCheck()
    {
        var checkInterval = new WaitForSeconds(.5f);

#if RUBY_FRAMEWORK_ADS
        while (GameManager.Instance.enabled)
        {
            yield return checkInterval;

            if (RubyGames.Framework.RubyFramework.IsBannerLoaded)
            {
                RubyGames.Framework.RubyFramework.ShowBanner();
            }
        }
#endif

        yield return null;
    }
}