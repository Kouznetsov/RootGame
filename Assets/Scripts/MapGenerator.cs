using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Node nodePrefab;
    [SerializeField] private float nodeSize;
    [SerializeField] private float gap;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Vector2 startNode;
    [SerializeField] private Vector3 endNode;
    private List<Node> _instances = new();

    private void Start()
    {
        Generate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var i in _instances)
            {
                Destroy(i.gameObject);
            }
            _instances.Clear();
            Generate();
        }
    }

    public void Generate()
    {
        var currentPosition = Vector3.zero;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                currentPosition.x = (x + gap) * nodeSize;
                currentPosition.z = (y + gap) * nodeSize;
                _instances.Add(Instantiate(nodePrefab, currentPosition, Quaternion.identity));
            }
        }
    }
}