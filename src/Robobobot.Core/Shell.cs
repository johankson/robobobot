using System.Numerics;

namespace Robobobot.Core;

public class Shell
{
    public Shell(Vector2 location, Player firedBy, Vector2 direction)
    {
        Location = location;
        FiredBy = firedBy;
        Direction = Vector2.Normalize(direction);
    }
    
    public Vector2 Location { get; set; }
    
    public Player FiredBy { get; set; }

    public Vector2 Direction { get; set; }

    public float Speed { get; set; } = 0.01f;
    
    public void Update(float elapsedGameTime, Battle battle)
    {
        // move
        Location += Direction * Speed;

        // check for collision (since we don't have any collision manager)
        foreach (var player in battle.Players)
        {
            if (player.Location.Is(Location))
            {
                
            }
        }
    }
}