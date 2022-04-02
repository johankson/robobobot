namespace Robobobot.Core.Actions;

public record MoveExecutionResult : ActionExecutionResult
{
    public Location? FinalLocation { get; init; }
}