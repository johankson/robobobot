namespace Robobobot.Core;

public class BattleSettings
{
    public MovementDurations MovementDurations { get; set; } = new ();

    /// <summary>
    /// The turret is always in the same direction as the tank.
    /// </summary>
    /// <remarks>Hard mode :)</remarks>
    public bool LockedTurret { get; set; } = false;

    public static BattleSettings Default => new ()
    {
        MovementDurations = new MovementDurations
        {
            AimDurationPerDegree = 10,
            MoveOverLandInMilliseconds = 50,
            MoveThroughForrestInMilliseconds = 100,
            FailureToMoveInMilliseconds = 140
        }
    };
    
    /// <summary>
    /// The amount of time with no incoming traffic to consider this game stale and
    /// mark it for removal from the server.
    /// </summary>
    public int StaleTimeoutInMinutes { get; set; } = 5;
}

public class MovementDurations
{
    public int MoveOverLandInMilliseconds { get; set; }
    public int MoveThroughForrestInMilliseconds { get; set; }
    public int FailureToMoveInMilliseconds { get; set; }
    
    /// <summary>
    /// The time to aim per degree of movement.
    /// </summary>
    public int AimDurationPerDegree { get; set; }
}