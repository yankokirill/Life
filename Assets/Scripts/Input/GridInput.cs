using Unity.VisualScripting;
using UnityEngine;

public class GridInput : MonoBehaviour
{
    private int width;
    private int height;
    private Bounds bounds;

    public void Initialize(Bounds bounds, int width, int height) {
        this.bounds = bounds;
        this.width = width;
        this.height = height;
    }

    public Vector2Int GetGridCoordinates()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        float relativeX = (mouseWorldPos.x - bounds.min.x) / bounds.size.x;
        float relativeY = (mouseWorldPos.y - bounds.min.y) / bounds.size.y;

        int gridX = Mathf.Clamp(Mathf.FloorToInt(relativeX * width), 0, width - 1);
        int gridY = Mathf.Clamp(Mathf.FloorToInt(relativeY * height), 0, height - 1);

        return new Vector2Int(gridX, gridY);
    }
}