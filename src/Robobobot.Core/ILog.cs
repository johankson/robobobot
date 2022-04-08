namespace Robobobot.Core;

public interface ILog
{
    IEnumerable<string> GetLast(int count);

    void Log(string message);
}