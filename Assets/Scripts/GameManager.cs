using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gcTimerTxt;
    [SerializeField] private TMP_Text timeLeftTxt;
    [SerializeField] private TMP_Text hitsLeftTxt;
    [SerializeField] private Slider fillSlider;
    [SerializeField] private int gcValue;
    [SerializeField] private int gcFrequency;
    [SerializeField] private int maxTime;
    [SerializeField] private int authorizedHits;
    [SerializeField] private float fillBarBySecond;
    [SerializeField] private MapManager mapManager;

    private bool _gameEnded;
    private float _timeLeft;
    private float _timeBeforeGC;
    private int _hitsLeft;

    private void Start()
    {
        fillSlider.value = 0;
        _timeBeforeGC = gcFrequency;
        _timeLeft = maxTime;
        _hitsLeft = authorizedHits;
        hitsLeftTxt.text = $"{_hitsLeft}";
        StartCoroutine(GameTimer());
        StartCoroutine(GCTimer());
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        mapManager.OnMapAltered += OnMapAltered;
    }

    private void OnDisable()
    {
        mapManager.OnMapAltered -= OnMapAltered;
    }

    private void Update()
    {
        if (mapManager.isFluxGoingThrough && !_gameEnded)
        {
            fillSlider.value += fillBarBySecond * Time.deltaTime;
            if (fillSlider.value >= fillSlider.maxValue)
                Win();
        }
    }

    private void Win()
    {
        // WIN !
        _gameEnded = true;
        Debug.Log("You won");
        Time.timeScale = 0;
    }

    private void Lose()
    {
        // LOSE !
        _gameEnded = true;
        Debug.Log("Lost");
        Time.timeScale = 0;
    }

    private void OnMapAltered()
    {
        _hitsLeft -= 1;
        if (_hitsLeft <= 0)
            Lose();
        hitsLeftTxt.text = $"{_hitsLeft}";
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