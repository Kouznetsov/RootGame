using UnityEngine;
using UnityEngine.UI;

public class TimerStopwatch : MonoBehaviour
{
    private float _timeLeft;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        _timeLeft = GameManager.LevelSo.maxTime;
    }

    private void Update()
    {
        _timeLeft -= Time.deltaTime;
        image.fillAmount = _timeLeft / GameManager.LevelSo.maxTime;
    }
}
