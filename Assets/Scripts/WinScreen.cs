using Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private LevelsListSo levelsListSo;
    

    public void OnHomePress()
    {
        SceneManager.LoadScene("Home");
    }

    public void OnRetryPress()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnNextLevelPress()
    {
        if (MapManager.levelSo.levelIndex >= levelsListSo.levelsList.Count)
        {
            OnRetryPress();
        }
        else
        {
            var currentLevel = MapManager.levelSo; 
            MapManager.levelSo = levelsListSo.levelsList[currentLevel.levelIndex + 1];
            GameManager.LevelSo = levelsListSo.levelsList[currentLevel.levelIndex + 1];
            SceneManager.LoadScene("Game");
        }
    }
}
