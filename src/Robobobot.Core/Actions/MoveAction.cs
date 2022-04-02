using Robobobot.Core.Extensions;
namespace Robobobot.Core.Actions;

public class MoveAction : ActionBase
{
    private readonly Player player;
    private readonly MoveDirection moveDirection;

    public MoveAction(Player player, MoveDirection moveDirection)
    {
        this.player = player;
        this.moveDirection = moveDirection;
    }

    public override Task<ActionExecutionResult> Execute(Battle battle)
    {
        var targetLocation = player.Location + moveDirection;
        var targetCell = battle.BattleField.GetCell(targetLocation);
        
        if (targetCell.IsMovableTo())
        {
            player.Location = targetLocation;
            var successResult = new MoveExecutionResult()
            {
                ExecutionDuration = targetCell.ResolveDuration(battle),
                FinalLocation = targetLocation,
                Success = true
            };
            
            return Task.FromResult<ActionExecutionResult>(successResult);
        }
        
        var failureResult = new MoveExecutionResult()
        {
            ExecutionDuration = battle.Settings.MovementDurations.FailureToMoveInMilliseconds,
            FinalLocation = player.Location,
            Success = false
        };
            
        return Task.FromResult<ActionExecutionResult>(failureResult);
    }
}