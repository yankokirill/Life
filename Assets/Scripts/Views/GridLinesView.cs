using Unity.VisualScripting;
using UnityEngine;

public class GridLinesView : MonoBehaviour
{
    [SerializeField] private Sprite gridSpriteOne;
    [SerializeField] private Sprite gridSpriteTwo;
    
    private SpriteRenderer spriteRenderer;
    private Bounds bounds;

    public void Initialize(Bounds bounds)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.bounds = bounds;
        SetGridOne();
    }

    public void SetGridTwo()
    {
        spriteRenderer.sprite = gridSpriteTwo;
        ApplyScaling();
    }

    public void SetGridOne()
    {
        spriteRenderer.sprite = gridSpriteOne;
        ApplyScaling();
    }

    private void ApplyScaling()
    {
        spriteRenderer.sortingOrder = 1;

        float scaleX = bounds.size.x / gridSpriteOne.bounds.size.x;
        float scaleY = bounds.size.y / gridSpriteOne.bounds.size.y;

        transform.localScale = new Vector3(scaleX, scaleY, 1f);
        transform.position = bounds.center;
    }
}
