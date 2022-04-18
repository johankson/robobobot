using System.Numerics;

namespace Robobobot.Core;

public class Shell
{
    public Shell(Vector2 location, Player firedBy, Vector2 direction)
    {
        startLocation = location;
        Location = location;
        FiredBy = firedBy;
        Direction = Vector2.Normalize(direction);
    }
    
    public Vector2 Location { get; set; }
    
    public Player FiredBy { get; set; }

    public Vector2 Direction { get; set; }

    public float Speed { get; set; } = 0.01f;

    private Vector2 startLocation;
    
    /// <summary>
    /// This shell has done it's job and should be deleted.
    /// </summary>
    public bool MarkForDeletion = false;
    
    public void Update(float elapsedGameTime, Battle battle)
    {
        // move
        Location += Direction * Speed * elapsedGameTime;

        // Check for collision (since we don't have any collision manager)
        foreach (var player in battle.Players)
        {
            // Don't shoot yourself
            if (player != FiredBy)
            {
                continue;
            }
            
            // Check range
            var travelledDistance = (Location - startLocation).Length();
            if (travelledDistance > battle.Settings.ShellRange)
            {
                // todo, the range should be read at creation perhaps and passed into the object.
                MarkForDeletion = true;
                return;
            }
            
            if (player.Location.Is(Location))
            {
                // bam!
                MarkForDeletion = true;
                player.HitByShell(this);
                return;
            }
        }
    }
}