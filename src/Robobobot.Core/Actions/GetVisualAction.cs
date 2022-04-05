using Robobobot.Core.Models;
using Robobobot.Server.Services;

namespace Robobobot.Core.Actions;

public class GetVisualAction : ActionBase
{
    private readonly string? playerToken;
    private Player? player;

    public GetVisualAction(Player player)
    {
        this.player = player;
    }

    public GetVisualAction(string playerToken)
    {
        this.playerToken = playerToken;
    }
    
    public override Task<ActionExecutionResult> Execute(Battle battle)
    {
        player ??= battle.Players.FirstOrDefault(p => p.Token == playerToken);

        if (player == null) throw new Exception("Can't resolve player");
        var battleField = battle.Renderer.RenderVisualBattlefieldPlayer(player);
        
        var result = new GetVisualExecutionResult()
        {
            BattleField = battleField,
            ExecutionDuration = 25,
            Success = true
        };

        return Task.FromResult<ActionExecutionResult>(result);
    }
}