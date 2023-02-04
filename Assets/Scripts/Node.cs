using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Node : MonoBehaviour
{
    [SerializeField] private List<NodeState> possibleStates;

    private int _stateIndex;
    private Material _material;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        SetState(Random.Range(0, possibleStates.Count));
    }

    public void SetNextState()
    {
        SetState(_stateIndex + 1 >= possibleStates.Count ? 0 : _stateIndex + 1);
    }
    
    public void SetState(int stateIndex)
    {
        _stateIndex = stateIndex;
        _material.color = possibleStates[stateIndex].color;
    }
}