using System.Numerics;
namespace Robobobot.Core.Models;

public record Location(int X, int Y)
{
    public static Location operator +(Location location, MoveDirection direction)
    {
        var (x, y) = location;

        var result = direction switch
        {
            MoveDirection.North => new Location(x, y-1),
            MoveDirection.NorthEast => new Location(x+1, y-1),
            MoveDirection.East => new Location(x+1, y),
            MoveDirection.SouthEast => new Location(x+1, y+1),
            MoveDirection.South => new Location(x, y+1),
            MoveDirection.SouthWest => new Location(x-1, y+1),
            MoveDirection.West => new Location(x-1, y),
            MoveDirection.NorthWest => new Location(x-1, y-1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return result;
    }
    
    public bool Is(int x, int y) => X == x && Y == y;
    
    public bool Is(Vector2 location) => Is((int)location.X, (int)location.Y);
    
    public static Location Create(int x, int y) => new(x, y);
    
    public static Location Random(int maxX, int maxY) => new(System.Random.Shared.Next(maxX), System.Random.Shared.Next(maxY));
}