using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private PatternManager patternManager;
    [SerializeField] private GridView gridView;
    [SerializeField] private GridInput gridInput;
    [SerializeField] private Transform quadTransform;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private int simulationStepsPerSecond = 8;
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;

    private float editTimer = 0f;
    private int editTick = 32;

    private float timer = 0f;
    private int maxSimulationStepsPerSecond = 512;
    private int minSimulationStepsPerSecond = 1;
    private bool simulationIsRunning = false;

    private int framesPerSecond = 0;
    private int totalStepsInInterval = 0;
    private float measurementInterval = 1f;
    private float measurementTimer = 0f;
    private Vector2Int? lastGridCoords = null;

    void Start()
    {
        quadTransform.localScale = new Vector3(width, height, 1f);
        var renderer = quadTransform.GetComponent<Renderer>();
        var bounds = renderer.bounds;
        gridInput.Initialize(bounds, width, height);
        gridView.Initialize(width, height);
        cameraController.Initialize(bounds);
    }

    void Update()
    {
        HandleInput();
        HandleSimulation();
    }

    private void HandleInput()
    {
        HandleExit();
        HandleGameState();
        HandleRandomize();
        HandleReset();
        HandlePatterns();
        HandleSimulationSpeed();
        HandleEdit();
    }

    void HandleExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void HandleGameState()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (simulationIsRunning)
            {
                gridView.StopSimulation();
                simulationIsRunning = false;
            }
            else
            {
                gridView.StartSimulation();
                simulationIsRunning = true;
            }
            timer = 0f;
        }
    }

    private void HandleRandomize()
    {
        if (!simulationIsRunning && Input.GetKeyDown(KeyCode.R))
        {
            gridView.RandomizeGrid();
        }
    }

    private void HandleReset()
    {
        if (!simulationIsRunning && Input.GetKeyDown(KeyCode.Backspace))
        {
            gridView.ResetGrid();
        }
    }

    private void SetPattern(Cell[] cells)
    {
        var gridCoords = gridInput.GetGridCoordinates();
        for (int i = 0; i < cells.Length; i++)
        {
            gridView.SetCell((gridCoords.x + cells[i].x + width) % width, (gridCoords.y + cells[i].y + height) % height, true);
        }
    }

    private void HandlePatterns()
    {
        if (simulationIsRunning)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) SetPattern(GliderPattern.GetCells());
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetPattern(LwssPattern.GetCells());
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetPattern(BlinkerPattern.GetCells());
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetPattern(ToadPattern.GetCells());
        if (Input.GetKeyDown(KeyCode.Alpha5)) SetPattern(BeaconPattern.GetCells());
        if (Input.GetKeyDown(KeyCode.Alpha6)) SetPattern(PulsarPattern.GetCells());
        if (Input.GetKeyDown(KeyCode.Alpha7)) SetPattern(GosperGliderGunPattern.GetCells());
        if (Input.GetKeyDown(KeyCode.Alpha8)) SetPattern(EaterPattern.GetCells());
        if (Input.GetKeyDown(KeyCode.Alpha9)) SetPattern(patternManager.GetPattern("SpaceShipGunP690"));
    }

    private void HandleSimulationSpeed()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            simulationStepsPerSecond = Mathf.Min(simulationStepsPerSecond * 2, maxSimulationStepsPerSecond);
            Debug.Log("Steps per second: " + simulationStepsPerSecond);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            simulationStepsPerSecond = Mathf.Max(simulationStepsPerSecond / 2, minSimulationStepsPerSecond);
            Debug.Log("Steps per second: " + simulationStepsPerSecond);
        }
    }

    private void HandleEdit()
    {
        if (simulationIsRunning)
        {
            lastGridCoords = null;
            return;
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            bool newState = Input.GetMouseButton(0);
            Vector2Int gridCoords = gridInput.GetGridCoordinates();

            if (lastGridCoords.HasValue)
            {
                Vector2Int last = lastGridCoords.Value;
                DrawLineBetween(last, gridCoords, newState);
            }

            gridView.SetCell(gridCoords.x, gridCoords.y, newState);
            lastGridCoords = gridCoords;
        }
        else
        {
            lastGridCoords = null;
        }

        editTimer += Time.deltaTime;
        if (editTimer >= 1f / editTick)
        {
            editTimer = 0f;
            gridView.Display();
        }
    }

    private void DrawLineBetween(Vector2Int start, Vector2Int end, bool alive)
    {
        int x0 = start.x;
        int y0 = start.y;
        int x1 = end.x;
        int y1 = end.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1;
        int sy = (y0 < y1) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            gridView.SetCell(x0, y0, alive);
            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }
    }

    private void HandleSimulation()
    {
        if (!simulationIsRunning)
            return;

        timer += Time.deltaTime;
        measurementTimer += Time.deltaTime;
        float stepInterval = 1f / simulationStepsPerSecond;

        while (timer >= stepInterval)
        {
            gridView.StepSimulation();
            timer -= stepInterval;
            totalStepsInInterval++;
        }

        framesPerSecond++;
        if (measurementTimer > measurementInterval)
        {
            Debug.Log($"Simulation steps per second: {totalStepsInInterval}");
            Debug.Log($"FPS: {framesPerSecond}");
            measurementTimer = 0f;
            totalStepsInInterval = 0;
            framesPerSecond = 0;
        }
        gridView.Display();
    }
}
