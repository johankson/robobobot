namespace Robobobot.Core;

public class BattleSettings
{
    public ExecutionDurations ExecutionDurations { get; set; } = new ();

    /// <summary>
    /// The turret is always in the same direction as the tank.
    /// </summary>
    /// <remarks>Hard mode :)</remarks>
    public bool LockedTurret { get; set; } = false;

    public static BattleSettings Default => new ()
    {
        ExecutionDurations = new ExecutionDurations
        {
            AimDurationPerDegree = 10,
            MoveOverLandInMilliseconds = 50,
            MoveThroughForrestInMilliseconds = 100,
            MoveFailureInMilliseconds = 140,
            GetReadingInMilliseconds = 50,
            FireInMilliseconds = 1000
        }
    };
    
    /// <summary>
    /// The amount of time with no incoming traffic to consider this game stale and
    /// mark it for removal from the server.
    /// </summary>
    public int StaleTimeoutInMinutes { get; set; } = 5;
    
    /// <summary>
    /// If set to true, then the players are randomized on the available start
    /// positions that are set for the current map.
    /// </summary>
    public bool RandomizeStartPositionAssignment { get; set; }
    
    /// <summary>
    /// The number of units that the standard shell can travel in open
    /// landscape.
    /// 
    /// Default value is 10.
    /// </summary>
    public float ShellRange { get; set; } = 10;
}

public class ExecutionDurations
{
    public int MoveOverLandInMilliseconds { get; set; }
    public int MoveThroughForrestInMilliseconds { get; set; }
    public int MoveFailureInMilliseconds { get; set; }
    
    /// <summary>
    /// The time to aim per degree of movement.
    /// </summary>
    public int AimDurationPerDegree { get; set; }
    
    public int GetReadingInMilliseconds { get; set; }
    
    /// <summary>
    /// The time it takes to handle the fire action.
    /// </summary>
    public int FireInMilliseconds { get; set; }
}

