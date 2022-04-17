using System.ComponentModel;
using Robobobot.Core.Models;
namespace Robobobot.Core.Actions;

public class GetReadingsAction : ActionBase
{
    private Player? player;
    private readonly string? playerToken;

    public const string TurretAngleMoniker = "TURRET_ANGLE";
    public const string TankHeadingMoniker = "TANK_HEADING";
    
    public GetReadingsAction(string playerToken)
    {
        this.playerToken = playerToken;
    }
    
    public GetReadingsAction(Player player)
    {
        this.player = player;
    }
    
    public override Task<ActionExecutionResult> Execute(Battle battle)
    {
        player ??= battle.Players.FirstOrDefault(p => p.Token == playerToken);

        if (player == null) throw new Exception("Can't resolve player");
        
        var result = new GetReadingsResult()
        {
            ExecutionDuration = battle.Settings.ExecutionDurations.GetReadingInMilliseconds,
            Success = true
        };
        
        result.Values.Add(TurretAngleMoniker, player.TurretDegree.ToString());
        result.Values.Add(TankHeadingMoniker, player.TankHeading.ToString());

        return Task.FromResult<ActionExecutionResult>(result);
    }
}