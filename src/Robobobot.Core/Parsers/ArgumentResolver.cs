namespace Robobobot.Core.Parsers;

public abstract class ArgumentResolver
{
    public abstract string Name { get; }

    public abstract void Resolve(BattleField battleField, List<string> argument);
}