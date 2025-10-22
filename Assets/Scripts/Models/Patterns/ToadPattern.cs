public static class ToadPattern
{
    public static Cell[] GetCells()
    {
        return new Cell[]
        {
            new Cell(1, 0),
            new Cell(2, 0),
            new Cell(3, 0),
            new Cell(0, 1),
            new Cell(1, 1),
            new Cell(2, 1)
        };
    }
}