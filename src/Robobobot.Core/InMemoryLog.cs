namespace Robobobot.Core;

public class InMemoryLog : ILog
{
    private readonly List<string> log = new();

    public IEnumerable<string> GetLast(int count)
    {
        var lastIndex = log.Count - 1;
        var startIndex = Math.Max(0, lastIndex - count);
        
        for (var i = startIndex; i<=lastIndex;i++)
        {
            yield return log[i];
        }
    }

    public void Log(string message)
    {
        log.Add($"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.FFF} {message}");
    }
}