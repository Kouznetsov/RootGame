using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Node nodePrefab;
    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject endPrefab;
    [SerializeField] private float nodeSize;
    [SerializeField] private float gap;
    [SerializeField] private int width;
    [SerializeField] private int height;

    private readonly List<Node> _instances = new();
    private GameObject _startInstance;
    private GameObject _endInstance;

    public delegate void MapGeneratedEvent(int w, int h, float nodeSize, float gap);

    public MapGeneratedEvent OnMapGenerated;

    private void Start()
    {
        Generate();
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
        var startNode = Random.Range(0, width - 1);
        var endNode = Random.Range(0, width - 1);
        
        _startInstance = Instantiate(startPrefab);
        _endInstance = Instantiate(endPrefab);
        currentPosition.z = -nodeSize - gap;
        currentPosition.x = startNode * (nodeSize + gap);
        _startInstance.transform.position = currentPosition;
        currentPosition.z = height * (nodeSize + gap);
        currentPosition.x = endNode * (nodeSize + gap);
        _endInstance.transform.position = currentPosition;
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
                _instances.Add(Instantiate(nodePrefab, currentPosition, Quaternion.identity));
            }
        }

        OnMapGenerated?.Invoke(width, height, nodeSize, gap);
    }
}