namespace Robobobot.Core.Models;

public record AimActionResult : ActionExecutionResult
{
    public int FinalAngle { get; set; }
}