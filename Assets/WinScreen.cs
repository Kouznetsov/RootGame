using Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text seconds;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private LevelsListSo levelsListSo;
    
    private void Start()
    {
        seconds.text = $"{gameManager.GetTimeLeft()}";
    }

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
            GameManager.levelSo = levelsListSo.levelsList[currentLevel.levelIndex + 1];
            SceneManager.LoadScene("Game");
        }
    }
}
