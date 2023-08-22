using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [ReadOnly] public LevelData currentLevel;

    [SerializeField] private List<LevelData> levels;

    [SerializeField] private bool dontLoadLevels;

    public LevelManager Initialize()
    {
#if UNITY_EDITOR
        if (dontLoadLevels)
        {
            currentLevel = new LevelData
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

        currentLevel = new LevelData
        {
            levelId = levelToInitialize.levelId,
            levelController = Instantiate(levelToInitialize.levelController).Initialize()
        };

        return this;
    }
    
    public static void LoadNextLevel()
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