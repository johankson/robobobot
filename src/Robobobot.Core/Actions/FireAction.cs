using System.Numerics;
using Robobobot.Core.Models;
using Robobobot.Core.Utils;
namespace Robobobot.Core.Actions;

public class FireAction : ActionBase
{
    private Player? player;
    private readonly string? playerToken;
    
    public override Task<ActionExecutionResult> Execute(Battle battle)
    { 
        player ??= battle.Players.FirstOrDefault(p => p.Token == playerToken);
        if (player == null) throw new Exception("Can't resolve player");

        var result = new FireActionResult()
        {
            Success = true,
            ExecutionDuration = battle.Settings.ExecutionDurations.FireInMilliseconds,
        };

        var direction = Vector2.Transform(Vector2.UnitY, Matrix3x2.CreateRotation(-VectorUtil.DegreeToRadian(player.TankHeading + player.TurretDegree)));
        var shell = new Shell(new Vector2(player.Location.X + 0.5f, player.Location.Y + 0.5f), player, direction);
        battle.AddShell(shell);

        return Task.FromResult<ActionExecutionResult>(result);
    }
    
    public FireAction(string playerToken)
    {
        this.playerToken = playerToken;
    }
    
    public FireAction(Player player)
    {
        this.player = player;
    }
}