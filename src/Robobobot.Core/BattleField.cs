using Robobobot.Core.Models;
using Robobobot.Core.Parsers;
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
        var parser = new BattleFieldParser();
        var (field, width, height, argumentsGroups) = parser.Parse(map);
        
        var rows = field.Split(Environment.NewLine);
        var battleField = new BattleField(width, height);
        battleField.cells.ForEach((x, y, cell) => cell.Type = ResolveType(rows[y][x]));

        // Default values
        battleField.AddDefaultStartLocations();
        
        foreach (var group in argumentsGroups)
        {
            battleField.argumentResolvers.FirstOrDefault(e=>e.Name == group.Key)?.Resolve(battleField, group.Value);   
        }

        return battleField;
    }

    private readonly List<ArgumentResolver> argumentResolvers = new()
    {
        new StartPositionResolver()
    };
    
    /// <summary>
    /// Creates 8 start positions located along the edges of the map.
    /// </summary>
    public void AddDefaultStartLocations()
    {
        StartPositions = new List<Location>()
        {
            Location.Create(2, 2),
            Location.Create(Width / 2, 2),
            Location.Create(Width - 2, 2),
            Location.Create(2, Height / 2),
            Location.Create(Width / 2, Height / 2),
            Location.Create(2, Height - 2),
            Location.Create(Width / 2, Height - 2),
            Location.Create(Width - 2, Height - 2)
        };
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

    internal List<Location> StartPositions = new();

    public Location GetNextStartPosition(bool randomizeStartPositionAssignment)
    {
        var index = randomizeStartPositionAssignment ? Random.Shared.Next(StartPositions.Count) : 0;
        var location = StartPositions[index];
        StartPositions.RemoveAt(index);
        return location;
    }
    public bool IsInsideMap(Location location) =>
        !(location.X < 0 || location.Y < 0 || location.X > Width - 1 || location.Y > Height - 1);
}
public enum CellType
{
    Land,
    Forrest,
    Mountain,
    Swamp
}