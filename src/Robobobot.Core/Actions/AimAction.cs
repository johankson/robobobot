using Robobobot.Core.Models;
namespace Robobobot.Core.Actions;

public class AimAction : ActionBase
{
    private Player? player;
    private readonly string? playerToken;

    public AimAction(string playerToken, int deltaAngle)
    {
        this.playerToken = playerToken;
        DeltaAngle = deltaAngle;
    }
    
    public AimAction(Player player, int deltaAngle)
    {
        this.player = player;
        DeltaAngle = deltaAngle;
    }
    
    public override Task<ActionExecutionResult> Execute(Battle battle)
    {
        player ??= battle.Players.FirstOrDefault(p => p.Token == playerToken);
        if (player == null) throw new Exception("Can't resolve player");

        var result = new AimActionResult()
        {
            FinalAngle = Utils.AngleUtil.WrapAngle(player.TurretDegree + DeltaAngle),
            Success = true,
            ExecutionDuration = (Math.Abs(DeltaAngle) % 360) * battle.Settings.ExecutionDurations.AimDurationPerDegree
        };

        if (result.Success)
        {
            player.TurretDegree = result.FinalAngle;
        }

        return Task.FromResult<ActionExecutionResult>(result);
    }
    
    /// <summary>
    /// The amount of degrees to turn to turret, relative to the current position.
    /// </summary>
    public int DeltaAngle { get; set; }
}