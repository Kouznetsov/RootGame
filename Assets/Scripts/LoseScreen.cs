using Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{ public void OnHomePress()
    {
        SceneManager.LoadScene("Home");
    }

    public void OnRetryPress()
    {
        SceneManager.LoadScene("Game");
    }
}