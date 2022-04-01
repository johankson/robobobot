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
        var targetLocation = player.Location += moveDirection;

        var targetCell = battle.BattleField.GetCell(targetLocation);
        if (targetCell.IsMovableTo())
        {
            player.Location = targetLocation;
            var result = new MoveExecutionResult()
            {
                ExecutionDuration = 4000,
                NewLocation = targetLocation,
                Success = true,
            };
            
            return Task.FromResult<ActionExecutionResult>(result);
        }

        throw new NotImplementedException("Bad moves not implemented yet");
    }
}