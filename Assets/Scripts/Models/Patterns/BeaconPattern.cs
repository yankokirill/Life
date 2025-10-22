public static class BeaconPattern
{
    public static Cell[] GetCells()
    {
        return new Cell[]
        {
            new Cell(0, 0),
            new Cell(1, 0),
            new Cell(0, 1),
            new Cell(3, 3),
            new Cell(2, 3),
            new Cell(3, 2)
        };
    }
}