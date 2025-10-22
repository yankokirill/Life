using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public struct Cell
{
    public int x;
    public int y;
    public Cell(int x, int y) { this.x = x; this.y = y; }
}

public static class RleParser
{
    // Парсит текст RLE и возвращает массив живых клеток
    public static Cell[] Parse(string rleText)
    {
        if (string.IsNullOrEmpty(rleText)) return new Cell[0];

        string[] lines = rleText.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        List<string> bodyLines = new List<string>();

        foreach (var raw in lines)
        {
            string line = raw.Trim();
            if (line.Length == 0 || line.StartsWith("#")) continue; // игнорируем комментарии
            if (line.StartsWith("x")) continue; // заголовок
            bodyLines.Add(line);
        }

        string body = string.Join("", bodyLines).Trim();
        int excl = body.IndexOf('!');
        if (excl >= 0) body = body.Substring(0, excl + 1);

        List<Cell> cells = new List<Cell>();
        int x = 0, y = 0;
        int i = 0;

        while (i < body.Length)
        {
            if (char.IsWhiteSpace(body[i])) { i++; continue; }

            int num = 0;
            bool haveNum = false;
            while (i < body.Length && char.IsDigit(body[i]))
            {
                haveNum = true;
                num = num * 10 + (body[i] - '0');
                i++;
            }
            if (!haveNum) num = 1;

            if (i >= body.Length) break;
            char c = body[i];
            i++;

            if (c == 'b')
            {
                x += num;
            }
            else if (c == 'o')
            {
                for (int k = 0; k < num; k++)
                {
                    cells.Add(new Cell(x, y));
                    x++;
                }
            }
            else if (c == '$')
            {
                y += num;
                x = 0;
            }
            else if (c == '!')
            {
                break;
            }
            else
            {
                // игнорируем неизвестные символы
            }
        }

        return cells.ToArray();
    }
}

public class PatternManager : MonoBehaviour
{
    private Dictionary<string, Cell[]> patterns = new Dictionary<string, Cell[]>();
    [SerializeField] private TextAsset file;

    void Start()
    {
        LoadPatterns();
    }

    void LoadPatterns()
    {
        Debug.Log("Load patterns");

        // Загружаем один конкретный файл
        Cell[] cells = RleParser.Parse(file.text);
        patterns[file.name] = cells;
        Debug.Log($"Loaded pattern: {file.name}, cells count: {cells.Length}");
    }

    public Cell[] GetPattern(string name)
    {
        if (patterns.TryGetValue(name, out Cell[] p))
            return p;
        return new Cell[0];
    }
}
