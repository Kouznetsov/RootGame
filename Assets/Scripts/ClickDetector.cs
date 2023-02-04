using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask nodeLayerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, nodeLayerMask))
            {
                Debug.Log("Collided with a node !");
            }
        }
    }
}