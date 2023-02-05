using System.Collections;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gcTimerTxt;
    [SerializeField] private TMP_Text timeLeftTxt;
    [SerializeField] private Slider fillSlider;
    [SerializeField] private int gcValue;
    [SerializeField] private int gcFrequency;
    [SerializeField] private int maxTime;
    [SerializeField] private float fillBarBySecond;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private Image fillBar;
    public static LevelSo LevelSo;

    private bool _gameEnded;
    private float _timeLeft;
    private float _timeBeforeGC;

    private void Start()
    {
        fillBar.fillAmount = 0;
        gcValue = LevelSo.gcValue;
        gcFrequency = LevelSo.gcFrequency;
        maxTime = LevelSo.maxTime;
        fillBarBySecond = LevelSo.fillBarBySeconds;
        fillSlider.value = 0;
        _timeBeforeGC = gcFrequency;
        _timeLeft = maxTime;
        StartCoroutine(GameTimer());
        StartCoroutine(GCTimer());
        Time.timeScale = 1;
    }

    public int GetTimeLeft()
    {
        return Mathf.FloorToInt(_timeLeft);
    }

    private void Update()
    {
        if (mapManager.isFluxGoingThrough && !_gameEnded)
        {
            fillBar.fillAmount += (fillBarBySecond / 100) * Time.deltaTime;
            if (fillBar.fillAmount > .999f)
                Win();
            // fillSlider.value += fillBarBySecond * Time.deltaTime;
            // if (fillSlider.value >= fillSlider.maxValue)
            // Win();
        }
    }

    private void Win()
    {
        // WIN !
        _gameEnded = true;
        Debug.Log("You won");
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }

    private void Lose()
    {
        // LOSE !
        _gameEnded = true;
        Debug.Log("Lost");
        Time.timeScale = 0;
        loseScreen.SetActive(true);
    }

    private IEnumerator GameTimer()
    {
        while (_timeLeft > 0)
        {
            timeLeftTxt.text = $"{_timeLeft}";
            yield return new WaitForSeconds(1);
            _timeLeft -= 1;
        }

        Lose();
    }

    private IEnumerator GCTimer()
    {
        mapManager.CollectGarbage(gcValue);
        while (!_gameEnded)
        {
            gcTimerTxt.text = $"{_timeBeforeGC}";
            yield return new WaitForSeconds(1);
            _timeBeforeGC -= 1;
            if (_timeBeforeGC <= 0)
            {
                mapManager.CollectGarbage(gcValue);
                _timeBeforeGC = gcFrequency;
            }
        }
    }
}