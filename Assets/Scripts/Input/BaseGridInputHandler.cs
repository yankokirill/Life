using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class BaseGridInputHandler : MonoBehaviour
{
    protected BoxCollider2D boxCollider;

    protected Vector2Int GetGridCoordinates(int width, int height)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        float relativeX = (mouseWorldPos.x - boxCollider.bounds.min.x) / boxCollider.bounds.size.x;
        float relativeY = (mouseWorldPos.y - boxCollider.bounds.min.y) / boxCollider.bounds.size.y;

        int gridX = Mathf.FloorToInt(relativeX * width) + 1;
        int gridY = Mathf.FloorToInt(relativeY * height) + 1;
        return new Vector2Int(gridX, gridY);
    }
}