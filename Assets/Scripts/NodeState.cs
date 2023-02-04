using System;
using UnityEngine;

public enum NodeStateType
{
    Red,
    Player
}

[Serializable]
public class NodeState
{
    public Color color;
    public NodeStateType state;
}