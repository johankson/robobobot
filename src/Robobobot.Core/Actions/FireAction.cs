using Robobobot.Core.Models;
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