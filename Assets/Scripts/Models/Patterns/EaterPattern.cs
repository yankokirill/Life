public static class EaterPattern
{
    public static Cell[] GetCells()
    {
        return new Cell[]
        {
            new Cell(0, 2), new Cell(1, 2),
            new Cell(0, 1), new Cell(1, 1),
            new Cell(2, 0),
            new Cell(1, -1),
            new Cell(0, -2)
        };
    }
}