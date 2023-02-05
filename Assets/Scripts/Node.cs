using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private List<NodeState> possibleStates;
    public MapManager mapManager;
    public bool isPlayerState => possibleStates[_stateIndex].state == NodeStateType.Player && !_isLocked;
    public bool isStart;
    public bool isEnd;
    public bool isConducting;
    private bool _isLocked;
    private int _stateIndex;
    private Material _material;
    public List<Node> _neighbours = new();
    private int _locksLeft;
    [SerializeField] private Vector3 rotationOnTouch;
    [SerializeField] private float bumpValue;
    private bool _wasBumped;

    public void ClearBumpedStatus()
    {
        _wasBumped = false;
    }

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        SetState(0, false);
    }

    public void OnClick()
    {
        NodeClickSounds.instance.PlaySound();
        if (_isLocked)
        {
            // hit !
            _locksLeft--;
            CameraShakeManager.instance.Shake(5 - _locksLeft);
            if (_locksLeft <= 0)
            {
                Unlock();
                SetState(1, true);
            }
        }
        else
        {
            if (!isPlayerState)
            {
                if (mapManager == null)
                    mapManager = MapManager.instance;
                mapManager.ClearBumps();
                SetState(1, true);
                StartCoroutine(Bump(bumpValue));
            }
        }
    }

    private IEnumerator Bump(float value)
    {
        if (_wasBumped)
            yield break;
        _wasBumped = true;
        // transform.DOLocalJump(transform.position, -value, 1, .3f, false);
        yield return new WaitForSeconds(.05f);
        foreach (var neighbour in _neighbours)
        {
            StartCoroutine(neighbour.Bump(value * .7f));
        }
    }


    public void Lock()
    {
        _locksLeft = 3;
        _isLocked = true;
        isConducting = false;
        _material.color = Color.black;
    }

    public void Unlock()
    {
        if (!_isLocked)
            return;
        _locksLeft = 0;
        _isLocked = false;
        isConducting = false;
        SetState(0, false);
    }

    public void CheckConduction()
    {
        if (isStart)
        {
            isConducting = true;
            return;
        }

        if (_isLocked)
        {
            isConducting = false;
            return;
        }

        foreach (var neighbour in _neighbours)
        {
            if (neighbour.isConducting &&
                (possibleStates[_stateIndex].state == NodeStateType.Player || isEnd))
            {
                isConducting = true;
            }
        }

        if (isConducting)
            foreach (var neighbour in _neighbours)
            {
                if (neighbour.isPlayerState)
                    neighbour.isConducting = true;
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Node"))
        {
            var n = other.GetComponent<Node>();
            if (n != null)
                _neighbours.Add(n);
        }
    }

    private void SetState(int stateIndex, bool byPlayer)
    {
        if (_isLocked || stateIndex >= possibleStates.Count || isStart || isEnd)
            return;
        _stateIndex = stateIndex;
        _material.color = possibleStates[stateIndex].color;
        // _material.DOColor(possibleStates[stateIndex].color, .3f);
        if (byPlayer)
            mapManager.OnMapAltered?.Invoke();
    }
}