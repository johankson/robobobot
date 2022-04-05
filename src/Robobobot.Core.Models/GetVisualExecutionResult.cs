namespace Robobobot.Core.Models;

public record GetVisualExecutionResult : ActionExecutionResult
{
    public string BattleField { get; init; } = string.Empty;
}