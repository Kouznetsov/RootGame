using System;
using System.Collections;
using System.Collections.Generic;
using CandyCoded.HapticFeedback;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR;

public class Node : MonoBehaviour
{
    [SerializeField] private List<NodeState> possibleStates;
    [SerializeField] private Texture2D smallCracks;
    [SerializeField] private Texture2D bigCracks;
    public ParticleSystem blackXplosion;
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
    [SerializeField] private float bumpValue;
    private Vector3 _initialPosition;
    private bool _wasBumped;
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

    public void ClearBumpedStatus()
    {
        _wasBumped = false;
    }

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        SetState(0, false);
    }

    private void Start()
    {
        _initialPosition = transform.position;
    }

    public void OnClick()
    {
        if (_isLocked)
        {
            // hit !
            blackXplosion.transform.position = transform.position;
            blackXplosion.Play();
            switch (_locksLeft)
            {
                case 3:
                    _material.color = Color.white;
                    _material.SetTexture(BaseMap, smallCracks);
                    HapticFeedback.LightFeedback();
                    break;
                case 2:
                    _material.SetTexture(BaseMap, bigCracks);
                    HapticFeedback.MediumFeedback();
                    break;
                case 1:
                    _material.color = Color.black;
                    _material.SetTexture(BaseMap, null);
                    HapticFeedback.HeavyFeedback();
                    break;
            }
            NodeClickSounds.instance.PlayBlackSound(_locksLeft);
            _locksLeft--;
            transform.DOShakePosition(.3f, (4 - _locksLeft) * .1f, 50, 45);
            CameraShakeManager.instance.Shake(5 - _locksLeft);
            if (_locksLeft <= 0)
            {
                Unlock();
                SetState(1, true);
                mapManager.ClearBumps();
                StartCoroutine(Bump(bumpValue));
            }
        }
        else
        {
            if (!isPlayerState)
            {
                NodeClickSounds.instance.PlayRedToGreenSound();
                mapManager.ClearBumps();
                SetState(1, true);
                StartCoroutine(Bump(bumpValue));
            }
        }
    }

    private IEnumerator Bump(float value)
    {
        if (_wasBumped || value < .1f)
            yield break;
        _wasBumped = true;
        var oldPos = transform.position;
        transform.DOJump(oldPos, -value, 1, .3f);
        yield return new WaitForSeconds(.05f);
        foreach (var neighbour in _neighbours)
        {
            StartCoroutine(neighbour.Bump(value * .5f));
        }

        yield return new WaitForSeconds(.3f);
        transform.position = _initialPosition;
    }


    public void Lock()
    {
        _locksLeft = 3;
        _isLocked = true;
        isConducting = false;
        _material.DOColor(Color.black, .3f);
        // _material.color = Color.black;
    }

    public void Unlock()
    {
        _material.SetTexture(BaseMap, null);
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
        _material.DOColor(possibleStates[stateIndex].color, .3f);
        if (byPlayer)
            mapManager.OnMapAltered?.Invoke();
    }
}