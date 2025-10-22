using UnityEngine;

public class PvPGridView : MonoBehaviour
{
    [SerializeField] private float cellSize = 0.4f;
    [SerializeField] private GridLinesView gridSprite;

    private Color[] pixelBuffer;
    private Texture2D texture;
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;

    private Color playerOneColor = Color.red;
    private Color playerTwoColor = Color.blue;
    private Color deadColor = Color.white;

    private PvPGameGrid grid;

    public void Initialize(PvPGameGrid gameGrid)
    {
        grid = gameGrid;

        InitializeTexture();
        InitializeSprite();
        gridSprite.Initialize(sprite.bounds);
    }

    private void InitializeTexture()
    {
        texture = new Texture2D(grid.Width, grid.Height, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        pixelBuffer = new Color[grid.Width * grid.Height];
        for (int i = 0; i < pixelBuffer.Length; i++)
        {
            pixelBuffer[i] = deadColor;
        }
        texture.SetPixels(pixelBuffer);
        texture.Apply();
    }

    private void InitializeSprite()
    {
        float pixelsPerUnit = 1f / cellSize;

        Rect rect = new Rect(0, 0, grid.Width, grid.Height);
        sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), pixelsPerUnit);

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = 0;

        Debug.Log($"Sprite bounds: {sprite.bounds}, Pixels per unit: {pixelsPerUnit}");
    }

    public void UpdateView()
    {
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                int index = y * grid.Width + x;

                CellState state = grid.GetCellState(x + 1, y + 1);
                if (state == CellState.Player1)
                    pixelBuffer[index] = playerOneColor;
                else if (state == CellState.Player2)
                    pixelBuffer[index] = playerTwoColor;
                else
                    pixelBuffer[index] = deadColor;
            }
        }

        texture.SetPixels(pixelBuffer);
        texture.Apply();
    }

    public Bounds Bounds()
    {
        return sprite.bounds;
    }

    public void SetGridOne()
    {
        gridSprite.SetGridOne();
    }

    public void SetGridTwo()
    {
        gridSprite.SetGridTwo();
    }
}
