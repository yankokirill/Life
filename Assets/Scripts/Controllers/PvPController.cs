using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PvPController : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 25;
    [SerializeField] private int height = 25;
    [SerializeField] private int startCells = 30;
    [SerializeField] private int stepsPerMove = 10;
    [SerializeField] private int turnsToPlay = 3;

    [Header("References")]
    [SerializeField] private PvPGridView gridView;
    [SerializeField] private PvPGridInputHandler gridInputHandler;
    [SerializeField] private TextMeshProUGUI stepsText;
    [SerializeField] private TextMeshProUGUI turnsText;
    [SerializeField] private GameOverScript gameOver;

    private PvPGameGrid gameGrid;
    private CellState currentCell = CellState.Player1;
    private int cntSteps = 0;
    private int cntTurns = 1;
    private bool simulationIsRunning = false;
    private float updateInterval = 0.256f;
    private float timer = 0f;

    void Start()
    {
        gameGrid = new PvPGameGrid(width, height);
        gridView.Initialize(gameGrid);
        gridInputHandler.Initialize(gridView.Bounds(), gameGrid, this);
        gridView.UpdateView();
        gameGrid.Randomize(startCells);
    }

    void Update()
    {
        if (simulationIsRunning)
        {
            HandleSimulation();
        }
        else
        {
            HandleInput();
        }
        gridView.UpdateView();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void HandleSimulation()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            gameGrid.UpdateGeneration();
            if (gameGrid.IsSameGeneration())
            {
                gameOver.Setup(gameGrid.GetCellCounter());
            }
            timer = 0f;
        }
    }

    public void MakeTurn(int x, int y)
    {
        if (simulationIsRunning || gameGrid.IsAliveCell(x, y))
            return;

        cntSteps++;
        gameGrid.SetCell(x, y, currentCell);
        if (cntSteps == stepsPerMove)
        {
            Color32 color;
            if (currentCell == CellState.Player1)
            {
                currentCell = CellState.Player2;
                color = new Color32(50, 50, 255, 255);
                gridView.SetGridTwo();
            }
            else
            {
                currentCell = CellState.Player1;
                cntTurns++;
                color = new Color32(255, 50, 50, 255);
                gridView.SetGridOne();
            }
            stepsText.color = color;
            turnsText.color = color;
            cntSteps = 0;

            if (cntTurns > turnsToPlay)
            {
                simulationIsRunning = true;
            } else
            {
                turnsText.text = cntTurns.ToString();
            }
        }
        stepsText.text = cntSteps.ToString();
    }
}