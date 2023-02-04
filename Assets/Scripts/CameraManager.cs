using System;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform camFocus;
    [SerializeField] private MapGenerator mapGenerator;
    private CinemachineVirtualCamera _vcam;
    private CinemachineTransposer _transposer;

    private void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _transposer = _vcam.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void OnEnable()
    {
        mapGenerator.OnMapGenerated += OnMapGenerated;
    }

    private void OnDisable()
    {
        mapGenerator.OnMapGenerated += OnMapGenerated;
    }

    private void OnMapGenerated(int w, int h, float nodeSize, float gap)
    {
        var camFocusTransform = camFocus.transform;
        var focusPosition = camFocusTransform.position;

        focusPosition.y = 0;
        focusPosition.x = (w - 1) * (nodeSize + gap) / 2f;
        focusPosition.z = (h - 1) * (nodeSize + gap) / 2f;
        camFocusTransform.position = focusPosition;
        _transposer.m_FollowOffset.y = Mathf.Max(h, w) * 3;
    }
}