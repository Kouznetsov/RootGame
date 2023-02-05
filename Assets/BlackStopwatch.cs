using UnityEngine;
using UnityEngine.UI;

public class BlackStopwatch : MonoBehaviour
{
    private float _timeLeft;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        _timeLeft = GameManager.LevelSo.gcFrequency;
    }

    private void Update()
    {
        _timeLeft -= Time.deltaTime;
        image.fillAmount = _timeLeft / GameManager.LevelSo.gcFrequency;
        if (_timeLeft <= 0)
            _timeLeft = GameManager.LevelSo.gcFrequency;
    }
}