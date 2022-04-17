namespace Robobobot.Core.Models;

public record GetReadingsResult : ActionExecutionResult
{
    public Dictionary<string, string> Values { get; set; } = new();
}