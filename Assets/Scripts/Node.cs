using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        SetState(Random.Range(0, possibleStates.Count), false);
    }

    public void SetNextState()
    {
        SetState(_stateIndex + 1 >= possibleStates.Count ? 0 : _stateIndex + 1, true);
    }

    public void Lock()
    {
        _isLocked = true;
        isConducting = false;
        _material.color = Color.black;
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

        Debug.Log($"I have {_neighbours.Count} neighbours");
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
        if (byPlayer)
            mapManager.OnMapAltered?.Invoke();
    }
}