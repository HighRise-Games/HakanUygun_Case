using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [ReadOnly] public LevelData currentLevelData;
    [ReadOnly] public LevelController currentLevel => currentLevelData.levelController;

    [SerializeField] private List<LevelData> levels;
    [SerializeField] private bool dontLoadLevels;

    public LevelManager Initialize()
    {
#if UNITY_EDITOR
        if (dontLoadLevels)
        {
            LogManager.LogError("Test Level Active, Don't ship game with this settings !", this);

            currentLevelData = new LevelData
            {
                levelId = "Test Level",
                levelController = FindObjectOfType<LevelController>()?.Initialize()
            };
            return this;
        }
#endif

        if (levels is null || levels.Count <= 0)
        {
            LogManager.LogError("Levels Missing !", this);
            return this;
        }

        var levelToInitialize = levels[DataManager.CurrentLevelIndex % levels.Count];

        currentLevelData = new LevelData
        {
            levelId = levelToInitialize.levelId,
            levelController = Instantiate(levelToInitialize.levelController).Initialize()
        };

        return this;
    }
    
    public static void ReloadScene()
    {
        DOTween.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

[Serializable]
public class LevelData
{
    public string levelId;
    public LevelController levelController;
}