using static Robobobot.Core.Extensions.CellExtensions;

namespace Robobobot.Core;

public class BattleField
{
    public int Width { get; }
    public int Height { get; }

    private readonly Cell[,] cells;

    public BattleField(int width, int height)
    {
        Width = width;
        Height = height;
        cells = new Cell[width, height];
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                cells[x, y] = new Cell
                {
                    X = x,
                    Y = y,
                    Type = CellType.Land
                };
            }
        }
    }

    public static BattleField FromPreExistingMap(string map)
    {
        // Figure out bounds
        map = map.TrimEnd(Environment.NewLine.ToCharArray());
        var rows = map.Split(Environment.NewLine);

        var width = rows.First().Length;
        var height = rows.Length;

        var field = new BattleField(width, height);
        field.cells.ForEach((x, y, cell) => cell.Type = ResolveType(rows[y][x]));

        return field;
    }

    public void ForEachCell(CellAction action)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                action(x, y, cells[x, y]);
            }
        }
    }
    
    private static CellType ResolveType(char c) => c switch
    {
        'M' => CellType.Mountain,
        'F' => CellType.Forrest,
        '.' => CellType.Land,
        'S' => CellType.Swamp,
        _ => throw new Exception($"Unsupported map token '{c}'")
    };

    public Cell GetCell(int x, int y) => cells[x, y];
    public Cell GetCell(Location location) => cells[location.X, location.Y];
}
public enum CellType
{
    Land,
    Forrest,
    Mountain,
    Swamp
}