namespace Robobobot.Core.Actions;

public record MoveExecutionResult : ActionExecutionResult
{
    public Location? NewLocation { get; init; }
}