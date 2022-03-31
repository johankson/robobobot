using Robobobot.Server.Services;

namespace Robobobot.Core.Actions;

public class GetVisualAction : ActionBase
{
    private readonly Player player;

    public GetVisualAction(Player player)
    {
        this.player = player;
    }
    
    public override Task<ActionExecutionResult> Execute(Battle battle)
    {
        var battleField = battle.Renderer.RenderVisualBattlefieldPlayer(player);
        
        var result = new GetVisualExecutionResult()
        {
            BattleField = battleField,
            ExecutionDuration = 2000,
            Success = true
        };

        return Task.FromResult<ActionExecutionResult>(result);
    }
}