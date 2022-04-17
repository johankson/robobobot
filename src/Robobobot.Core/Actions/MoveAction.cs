using Robobobot.Core.Extensions;
using Robobobot.Core.Models;
namespace Robobobot.Core.Actions;

public class MoveAction : ActionBase
{
    private readonly string playerToken;
    private readonly MoveDirection moveDirection;

    public MoveAction(string playerToken, MoveDirection moveDirection)
    {
        this.playerToken = playerToken;
        this.moveDirection = moveDirection;
    }

    public override Task<ActionExecutionResult> Execute(Battle battle)
    {
        var player = battle.FindPlayerByToken(playerToken);
        if (player == null) throw new Exception($"Can't find player with token '{playerToken}'");
        
        var targetLocation = player.Location + moveDirection;
        if (battle.BattleField.IsInsideMap(targetLocation))
        {
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