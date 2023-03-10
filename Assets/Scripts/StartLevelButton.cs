using Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevelButton : MonoBehaviour
{
    public LevelSo _levelSo;
    [SerializeField] private TMP_Text levelName;

    public void OnPress()
    {
        MapManager.levelSo = _levelSo;
        GameManager.LevelSo = _levelSo;
        SceneManager.LoadScene("Game");
    }

    public void Init(LevelSo level)
    {
        _levelSo = level;
        levelName.text = $"level {_levelSo.levelIndex}";
    }
}