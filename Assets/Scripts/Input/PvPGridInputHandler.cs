using UnityEngine;

public class PvPGridInputHandler : BaseGridInputHandler
{
    private PvPGameGrid grid;
    private PvPController controller;

    public void Initialize(Bounds bounds, PvPGameGrid grid, PvPController controller)
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = bounds.size;
        transform.position = bounds.center;
        this.grid = grid;
        this.controller = controller;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int gridCoords = GetGridCoordinates(grid.Width, grid.Height);
            controller.MakeTurn(gridCoords.x, gridCoords.y);
        }
    }
}
    