using Robobobot.Core.Models;
namespace Robobobot.Core.Parsers;

public class StartPositionResolver : ArgumentResolver
{
    public override string Name => "START_POSITIONS";
        
    public override void Resolve(BattleField battleField, List<string> argument)
    {
        battleField.StartPositions.Clear();
        var values = argument.Select(s =>
        {
            var arr = s.Split(',');
            return Location.Create(Int32.Parse(arr[0]), Int32.Parse(arr[1]));
        });
        battleField.StartPositions.AddRange(values);
    }
}