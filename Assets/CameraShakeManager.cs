using Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    private CinemachineVirtualCamera _vcam;
    private CinemachineBasicMultiChannelPerlin _noise;
    private float _amplitude;
    public static CameraShakeManager instance;

    private void Start()
    {
        instance = this;
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _noise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (_noise.m_AmplitudeGain > 0)
        {
            // _noise.m_AmplitudeGain -= Time.time;
        }
        else
        {
            _noise.m_AmplitudeGain = 0;
        }
    }

    public void Shake(int amplitude)
    {
        // _amplitude = amplitude;
        // _noise.m_AmplitudeGain = amplitude;
    }
}