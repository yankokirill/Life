using System;
using System.Threading.Tasks;
using UnityEngine;

public class LifeSeeder
{
    private GameGrid grid;

    // Параметры засева
    private int blockSize = 16;
    [Range(0f, 1f)] private float pActive = 0.1f;
    [Range(0f, 1f)] private float p0 = 0.35f;
    private float sigmaFactor = 0.7f;

    // Режим детерминизма
    public bool useFixedSeed = false;
    public int seed = 12345;

    public LifeSeeder(GameGrid grid)
    {
        this.grid = grid;
    }

    public void RandomizeParallel(Color[] pixelBuffer)
    {
        int W = grid.Width;
        int H = grid.Height;
        int B = blockSize;
        float sigma = B * sigmaFactor;
        Array.Fill(pixelBuffer, GameGrid.DeadColor);

        int nx = (W + B - 1) / B;
        int ny = (H + B - 1) / B;
        int totalBlocks = nx * ny;

        // Предвычисление Гаусса
        float twoSigma2 = 2f * sigma * sigma;
        float cx = (B - 1) * 0.5f;
        float cy = (B - 1) * 0.5f;
        float[] gauss = new float[B * B];
        for (int yy = 0; yy < B; yy++)
        {
            int off = yy * B;
            float dy = yy - cy;
            for (int xx = 0; xx < B; xx++)
            {
                float dx = xx - cx;
                float r2 = dx * dx + dy * dy;
                gauss[off + xx] = p0 * Mathf.Exp(-r2 / twoSigma2);
            }
        }

        int baseSeed = useFixedSeed ? seed : Environment.TickCount;
        System.Random baseRandom = new System.Random(baseSeed);

        // Создаём массив сидов заранее
        int[] blockSeeds = new int[totalBlocks];
        for (int i = 0; i < totalBlocks; i++)
        {
            blockSeeds[i] = baseRandom.Next();
        }

        // Параллельный обход по блокам
        Parallel.For(0, totalBlocks, (blockIndex) =>
        {
            System.Random rnd = new System.Random(blockSeeds[blockIndex]);

            if (rnd.NextDouble() >= pActive) return;

            int bx = blockIndex % nx;
            int by = blockIndex / nx;
            int x0 = bx * B;
            int y0 = by * B;
            int xMax = Math.Min(W, x0 + B);
            int yMax = Math.Min(H, y0 + B);

            for (int y = y0; y < yMax; y++)
            {
                int localY = y - y0;
                int rowBase = y * W;
                int gBase = localY * B;
                for (int x = x0; x < xMax; x++)
                {
                    int localX = x - x0;
                    float p = gauss[gBase + localX];
                    if (rnd.NextDouble() < p)
                    {
                        pixelBuffer[rowBase + x] = GameGrid.AliveColor;
                    }
                }
            }
        });
    }
}
