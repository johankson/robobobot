namespace Robobobot.Core;

public class BattleField
{
    public int Width { get; set; }
    public int Height { get; set; }

    private Cell[,] cells;

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

    public static BattleField FromPreExistingBattleField(string battleField)
    {
        // Figure out bounds
        battleField = battleField.TrimEnd(Environment.NewLine.ToCharArray());
        var rows = battleField.Split(Environment.NewLine);

        var width = rows.First().Length;
        var height = rows.Length;

        var field = new BattleField(width, height);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var row = rows[y];
                field.cells[x, y].Type = ResolveType(row[x]);
            }
        }

        return field;
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
}

public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public CellType Type { get; set; }

    public char GetCharType() => Type switch
    {
        CellType.Land => '.',
        CellType.Forrest => 'F',
        CellType.Mountain => 'M',
        CellType.Swamp => 'S',
        _ => throw new ArgumentOutOfRangeException()
    };
    public bool IsSeeThrough()=> Type switch
    {
        CellType.Land => true,
        CellType.Forrest => false,
        CellType.Mountain => false,
        CellType.Swamp => true,
        _ => throw new ArgumentOutOfRangeException()
    };
}

public enum CellType
{
    Land,
    Forrest,
    Mountain,
    Swamp
}