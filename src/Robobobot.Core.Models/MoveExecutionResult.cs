namespace Robobobot.Core.Models;

public record MoveExecutionResult : ActionExecutionResult
{
    public Location? FinalLocation { get; init; }
}