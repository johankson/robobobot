using Robobobot.Core.Models;
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

        var fieldPart = map.Split(Environment.NewLine + Environment.NewLine);
        var rows = fieldPart[0].Split(Environment.NewLine);

        var width = rows.First().Length;
        var height = rows.Length;

        var field = new BattleField(width, height);
        field.cells.ForEach((x, y, cell) => cell.Type = ResolveType(rows[y][x]));

        if (fieldPart.Any(f => f.StartsWith("START_POSITIONS")))
        {
            var values = fieldPart.First(f => f.StartsWith("START_POSITIONS")).Split(Environment.NewLine).Skip(1).Select(s =>
            {
                var arr = s.Split(',');
                return Location.Create(Int32.Parse(arr[0]), Int32.Parse(arr[1]));
            });
            field.startPositions.AddRange(values);
        }
        else
        {
            
        
        // no settings at all - quick start position randomization
            for (int i = 0; i < 10; i++)
            {
                var location = Location.Random(width, height);
                while (field.startPositions.Contains(location))
                {
                    location = Location.Random(width, height);
                }
                
                field.startPositions.Add(location);
            }
        }
        
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

    private List<Location> startPositions = new();

    public Location GetNextStartPosition(bool randomizeStartPositionAssignment)
    {
        var index = randomizeStartPositionAssignment ? Random.Shared.Next(startPositions.Count) : 0;
        var location = startPositions[index];
        startPositions.RemoveAt(index);
        return location;
    }
}
public enum CellType
{
    Land,
    Forrest,
    Mountain,
    Swamp
}