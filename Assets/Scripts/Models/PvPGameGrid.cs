using System.Collections.Generic;

public enum CellState
{
    Dead = 0,
    Player1 = 1,
    Player2 = 2
}

public struct CellCounter
{
    public int CountPlayer1;
    public int CountPlayer2;
    public CellCounter(int count1, int count2)
    {
        CountPlayer1 = count1;
        CountPlayer2 = count2;
    }

    public void Count(CellState state)
    {
        if (state == CellState.Player1)
            CountPlayer1++;
        else if (state == CellState.Player2)
            CountPlayer2++;
    }

    public int TotalCount()
    {
        return CountPlayer1 + CountPlayer2;
    }
}

public class PvPGameGrid
{
    private CellState[,] currentGrid;
    private CellState[,] nextGrid;
    private bool sameGeneration;
    private CellCounter counter;

    public int Width { get; }
    public int Height { get; }


    public PvPGameGrid(int width, int height)
    {
        Width = width;
        Height = height;

        currentGrid = new CellState[height + 2, width + 2];
        nextGrid = new CellState[height + 2, width + 2];
    }

    public bool IsSameGeneration()
    {
        return sameGeneration;
    }

    public CellCounter GetCellCounter()
    {
        return counter;
    }

    public void Reset()
    {
        for (int i = 1; i <= Height; i++)
        {
            for (int j = 1; j <= Width; j++)
            {
                currentGrid[i, j] = CellState.Dead;
            }
        }
    }

    public void Randomize(int count)
    {
        System.Random rand = new System.Random();

        Reset();

        var positions = new List<(int x, int y)>();
        for (int y = 1; y <= Height; y++)
        {
            for (int x = 1; x <= Width; x++)
            {
                positions.Add((y, x));
            }
        }

        for (int i = 0; i < positions.Count; i++)
        {
            int swapIndex = rand.Next(i, positions.Count);
            (positions[i], positions[swapIndex]) = (positions[swapIndex], positions[i]);
        }

        for (int i = 0; i < count; i++)
        {
            var (y, x) = positions[i];
            currentGrid[y, x] = CellState.Player1;
        }

        for (int i = count; i < 2 * count; i++)
        {
            var (y, x) = positions[i];
            currentGrid[y, x] = CellState.Player2;
        }
    }

    public CellState GetCellState(int x, int y)
    {
        return currentGrid[y, x];
    }

    public bool IsAliveCell(int x, int y)
    {
        return currentGrid[y, x] != CellState.Dead;
    }

    public void SetCell(int x, int y, CellState alive)
    {
        currentGrid[y, x] = alive;
    }

    private CellCounter CountNeighbors(int x, int y)
    {
        CellCounter ans = new CellCounter(0, 0);
        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dy == 0 && dx == 0)
                    continue;
                ans.Count(GetCellState(x + dx, y + dy));
            }
        }
        return ans;
    }

    public void UpdateGeneration()
    {
        bool sameGeneration = true;
        CellCounter counter = new CellCounter(0, 0);
        for (int y = 1; y <= Height; y++)
        {
            for (int x = 1; x <= Width; x++)
            {
                CellCounter neighbors = CountNeighbors(x, y);

                nextGrid[y, x] = currentGrid[y, x];
                if (!IsAliveCell(x, y) && neighbors.TotalCount() == 3)
                {
                    if (neighbors.CountPlayer1 > neighbors.CountPlayer2)
                        nextGrid[y, x] = CellState.Player1;
                    else
                        nextGrid[y, x] = CellState.Player2;

                    sameGeneration = false;
                }

                counter.Count(nextGrid[y, x]);
            }
        }
        (currentGrid, nextGrid) = (nextGrid, currentGrid);
        this.sameGeneration = sameGeneration;
        this.counter = counter;
    }
}