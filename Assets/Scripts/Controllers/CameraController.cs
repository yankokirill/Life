using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Параметры камеры")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 20f;

    [Header("Параметры перетаскивания")]
    [SerializeField] private float panSpeed = 1f;

    private Bounds gridBounds;
    private Camera cam;
    private Vector3 dragOrigin;
    private bool isDragging = false;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
        KeepCameraInBounds();
    }

    public void Initialize(Bounds bounds)
    {
        this.gridBounds = bounds;
        KeepCameraInBounds();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            // позиция мыши до зума (в мировых координатах)
            Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(Input.mousePosition);

            // изменяем orthographicSize с учётом ограничений
            cam.orthographicSize = Mathf.Clamp(
                cam.orthographicSize - scroll * zoomSpeed * 10f,
                minZoom,
                maxZoom
            );

            // позиция мыши после зума
            Vector3 mouseWorldAfter = cam.ScreenToWorldPoint(Input.mousePosition);

            // сдвигаем камеру так, чтобы под мышкой осталась та же точка
            transform.position += mouseWorldBefore - mouseWorldAfter;

            KeepCameraInBounds();
        }
    }

    private void HandlePan()
    {
        if (Input.GetMouseButtonDown(2)) // средняя кнопка мыши
        {
            isDragging = true;
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (!Input.GetMouseButton(2))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 newOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = dragOrigin - newOrigin;
            transform.position += delta * panSpeed;

            KeepCameraInBounds();

            // обновляем origin каждый кадр — чтобы не было рывков у границы
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void KeepCameraInBounds()
    {
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        Vector3 pos = transform.position;

        float clampedX = Mathf.Clamp(pos.x, gridBounds.min.x + halfWidth, gridBounds.max.x - halfWidth);
        float clampedY = Mathf.Clamp(pos.y, gridBounds.min.y + halfHeight, gridBounds.max.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, pos.z);
    }
}
