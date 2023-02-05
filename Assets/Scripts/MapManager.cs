using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Random = System.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Node nodePrefab;
    [SerializeField] private Node startPrefab;
    [SerializeField] private Node endPrefab;
    [SerializeField] private float nodeSize;
    [SerializeField] private float gap;
    [SerializeField] private int width;
    [SerializeField] private int height;
    public static LevelSo levelSo;
    public static MapManager instance;

    public bool isFluxGoingThrough => _endInstance.isConducting;

    private int _startNode;
    private int _endNode;
    private readonly Random _rnd = new();
    private readonly List<Node> _instances = new();
    private List<int> _lastLocked = new();
    private Node _startInstance;
    private Node _endInstance;

    public delegate void MapGeneratedEvent(int w, int h, float nodeSize, float gap);

    public delegate void MapAlteredEvent();

    public MapGeneratedEvent OnMapGenerated;
    public MapAlteredEvent OnMapAltered;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        height = levelSo.height;
        width = levelSo.width;
        Generate();
    }

    private void OnEnable()
    {
        OnMapAltered += CheckConducting;
    }

    private void OnDisable()
    {
        OnMapAltered -= CheckConducting;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Generate();
        }
    }

    private void ClearBoard()
    {
        foreach (var i in _instances)
            Destroy(i.gameObject);
        _instances.Clear();
        if (_endInstance != null)
            Destroy(_endInstance);
        if (_startInstance != null)
            Destroy(_startInstance);
    }

    private void GenerateStartAndEnd()
    {
        var currentPosition = Vector3.zero;
        _startNode = levelSo.startIndex < 0 ? UnityEngine.Random.Range(0, width - 1) : levelSo.startIndex;
        _endNode = levelSo.endIndex < 0 ? UnityEngine.Random.Range(0, width - 1) : levelSo.endIndex;
        _startInstance = Instantiate(startPrefab).GetComponent<Node>();
        _startInstance.mapManager = this;
        _endInstance = Instantiate(endPrefab).GetComponent<Node>();
        _endInstance.mapManager = this;
        currentPosition.z = -nodeSize - gap;
        currentPosition.x = _startNode * (nodeSize + gap);
        _startInstance.transform.position = currentPosition;
        currentPosition.z = height * (nodeSize + gap);
        currentPosition.x = _endNode * (nodeSize + gap);
        _endInstance.transform.position = currentPosition;
    }

    public void ClearBumps()
    {
        foreach (var node in _instances)
        {
            node.ClearBumpedStatus();
        }
    }

    public void CollectGarbage(int amount)
    {
        var indices = new List<int>();

        for (var i = 0; i < _instances.Count; i++)
        {
            if (!_lastLocked.Contains(i))
                indices.Add(i);
        }

        var chosenIndices = indices.OrderBy(x => _rnd.Next()).Take(amount);
        _lastLocked.Clear();
        _lastLocked.AddRange(chosenIndices);

        foreach (var node in _instances)
            node.Unlock();
        for (var i = 0; i < _instances.Count; i++)
        {
            if (_lastLocked.Contains(i))
                _instances[i].Lock();
        }
        CheckConducting();
    }

    public void Generate()
    {
        var currentPosition = Vector3.zero;

        ClearBoard();
        GenerateStartAndEnd();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                currentPosition.x = (x + x * gap) * nodeSize;
                currentPosition.z = (y + y * gap) * nodeSize;
                var toAdd = Instantiate(nodePrefab, currentPosition, Quaternion.identity);
                toAdd.mapManager = this;
                _instances.Add(toAdd);
            }
        }

        OnMapGenerated?.Invoke(width, height, nodeSize, gap);
        CheckConducting();
    }

    private void CheckConducting()
    {
        foreach (var node in _instances)
            node.isConducting = false;
        _endInstance.isConducting = false;
        foreach (var node in _instances)
            node.CheckConduction();
        _endInstance.CheckConduction();
    }
}