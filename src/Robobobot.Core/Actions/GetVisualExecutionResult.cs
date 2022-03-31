namespace Robobobot.Core.Actions;

public record GetVisualExecutionResult : ActionExecutionResult
{
    public string BattleField { get; set; } = string.Empty;
}