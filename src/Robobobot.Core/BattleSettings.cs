namespace Robobobot.Core;

public class BattleSettings
{
    public MovementDurations MovementDurations { get; set; } = new ();

    public static BattleSettings Default => new ()
    {
        MovementDurations = new MovementDurations
        {
            MoveOverLandInMilliseconds = 50,
            MoveThroughForrestInMilliseconds = 100,
            FailureToMoveInMilliseconds = 140
        }
    };
}

public class MovementDurations
{
    public int MoveOverLandInMilliseconds { get; set; }
    public int MoveThroughForrestInMilliseconds { get; set; }
    public int FailureToMoveInMilliseconds { get; set; }
}