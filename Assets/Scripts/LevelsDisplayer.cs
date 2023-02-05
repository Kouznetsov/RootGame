using Data;
using TMPro;
using UnityEngine;

public class LevelsDisplayer : MonoBehaviour
{
    [SerializeField] private LevelsListSo levelsList;
    [SerializeField] private GameObject levelButtonPrefab;

    private void Start()
    {
        foreach (var level in levelsList.levelsList)
        {
            var instance = Instantiate(levelButtonPrefab, transform).GetComponent<StartLevelButton>();

            instance.Init(level);
        }
    }
}
