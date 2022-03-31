namespace Robobobot.Core.Actions;

public abstract class ActionBase
{
    public abstract Task<ActionExecutionResult> Execute(Battle battle);

    public ActionExecutionResult? Result { get; set; }

    internal Action? CompleteCallback;

    public void OnComplete(Action completed)
    {
        CompleteCallback = completed;
    }
}