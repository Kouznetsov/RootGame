using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Node nodePrefab;
    [SerializeField] private Node startPrefab;
    [SerializeField] private Node endPrefab;
    [SerializeField] private float nodeSize;
    [SerializeField] private float gap;
    [SerializeField] private int width;
    [SerializeField] private int height;

    public bool isFluxGoingThrough => _endInstance.isConducting;

    private int _startNode;
    private int _endNode;
    private readonly System.Random _rnd = new();
    private readonly List<Node> _instances = new();
    private List<int> _lastLocked = new();
    private Node _startInstance;
    private Node _endInstance;

    public delegate void MapGeneratedEvent(int w, int h, float nodeSize, float gap);

    public delegate void MapAlteredEvent();

    public MapGeneratedEvent OnMapGenerated;
    public MapAlteredEvent OnMapAltered;

    private void Start()
    {
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
        _startNode = Random.Range(0, width - 1);
        _endNode = Random.Range(0, width - 1);

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

        // var nodes = _instances.OrderBy(x => _rnd.Next()).Take(amount);
        //
        // foreach (var node in _instances)
        // {
        //     node.Unlock();
        // }
        //
        // foreach (var node in nodes)
        // {
        //     node.Lock();
        // }

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