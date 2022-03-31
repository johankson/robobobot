namespace Robobobot.Core.Actions;

public record ActionExecutionResult
{
    public bool Success { get; set; }
    public int ExecutionDuration { get; set; }
    public List<State> PiggyBackedStateChanges { get; set; } = new();
}