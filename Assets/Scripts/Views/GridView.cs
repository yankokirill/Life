using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GridView : MonoBehaviour
{
    [Header("References")]
    public ComputeShader lifeCompute;
    public Material displayMaterial;

    private GameGrid grid;
    private int kernelID;
    private bool useAasInput = true;
    private bool simulationIsRunning = false;
    private RenderTexture currentFrame;

    private int width;
    private int height;

    public void Initialize(int width, int height)
    {
        this.width = width;
        this.height = height;
        grid = new GameGrid(width, height, 0.3f);
        kernelID = lifeCompute.FindKernel("LifeStep");
        displayMaterial.mainTexture = grid.GetTexture();
        currentFrame = grid.StateA;
    }

    public void StepSimulation()
    {
        RenderTexture src = useAasInput ? grid.StateA : grid.StateB;
        RenderTexture dst = useAasInput ? grid.StateB : grid.StateA;

        lifeCompute.SetTexture(kernelID, "State", src);
        lifeCompute.SetTexture(kernelID, "Result", dst);

        lifeCompute.SetInt("_Width", width);
        lifeCompute.SetInt("_Height", height);

        int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(height / 8.0f);
        lifeCompute.Dispatch(kernelID, threadGroupsX, threadGroupsY, 1);

        useAasInput = !useAasInput;
    }

    public void SetCell(int x, int y, bool state)
    {
        grid.SetCell(x, y, state);
    }

    public void StartSimulation()
    {
        simulationIsRunning = true;
        grid.SyncToRenderTexture(currentFrame);
    }

    public void StopSimulation()
    {
        simulationIsRunning = false;
        grid.SyncFromRenderTexture(currentFrame);
    }

    public void RandomizeGrid()
    {
        grid.Randomize();
    }

    public void ResetGrid()
    {
        grid.Reset();
    }

    public void Display()
    {
        if (simulationIsRunning)
        {
            currentFrame = useAasInput ? grid.StateA : grid.StateB;
            displayMaterial.mainTexture = currentFrame;
        } else
        {
            displayMaterial.mainTexture = grid.GetTexture();
        }
    }
}
