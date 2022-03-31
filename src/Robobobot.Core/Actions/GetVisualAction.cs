using Robobobot.Server.Services;

namespace Robobobot.Core.Actions;

public class GetVisualAction : ActionBase
{
    private readonly Player player;
    private readonly BattleRenderer renderer;

    public GetVisualAction(Player player, BattleRenderer renderer)
    {
        this.player = player;
        this.renderer = renderer;
    }
    
    public override Task<ActionExecutionResult> Execute()
    {
        var battleField = renderer.RenderVisualBattlefieldPlayer(player);
        
        var result = new GetVisualExecutionResult()
        {
            BattleField = battleField,
            ExecutionDuration = 2000,
            Success = true
        };

        return Task.FromResult<ActionExecutionResult>(result);
    }
}