public static class PulsarPattern
{
    public static Cell[] GetCells()
    {
        return new Cell[]
        {
            // Верхняя левая часть
            new Cell(2, 0), new Cell(3, 0), new Cell(4, 0),
            new Cell(0, 2), new Cell(5, 2),
            new Cell(0, 3), new Cell(5, 3),
            new Cell(0, 4), new Cell(5, 4),
            new Cell(2, 5), new Cell(3, 5), new Cell(4, 5),

            // Верхняя правая часть
            new Cell(8, 0), new Cell(9, 0), new Cell(10, 0),
            new Cell(7, 2), new Cell(12, 2),
            new Cell(7, 3), new Cell(12, 3),
            new Cell(7, 4), new Cell(12, 4),
            new Cell(8, 5), new Cell(9, 5), new Cell(10, 5),

            // Нижняя левая часть
            new Cell(2, 7), new Cell(3, 7), new Cell(4, 7),
            new Cell(0, 8), new Cell(5, 8),
            new Cell(0, 9), new Cell(5, 9),
            new Cell(0, 10), new Cell(5, 10),
            new Cell(2, 12), new Cell(3, 12), new Cell(4, 12),

            // Нижняя правая часть
            new Cell(8, 7), new Cell(9, 7), new Cell(10, 7),
            new Cell(7, 8), new Cell(12, 8),
            new Cell(7, 9), new Cell(12, 9),
            new Cell(7, 10), new Cell(12, 10),
            new Cell(8, 12), new Cell(9, 12), new Cell(10, 12)
        };
    }
}
