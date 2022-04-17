namespace Robobobot.Core.Parsers;

public class BattleFieldParser
{
    public (string map, int width, int height, Dictionary<string, List<string>> arguments) Parse(string level)
    {
        level = level.TrimEnd(Environment.NewLine.ToCharArray());

        var fieldPart = level.Split(Environment.NewLine + Environment.NewLine);
        var rows = fieldPart[0].Split(Environment.NewLine);

        var width = rows.First().Length;
        var height = rows.Length;

        var argumentGroups = new Dictionary<string, List<string>>();

        foreach (var part in fieldPart.Skip(1))
        {
            if (part.StartsWith("-") || string.IsNullOrWhiteSpace(part))
                continue;

            var arr = part.Split(Environment.NewLine);
            var name = arr[0];
            var values = arr.Skip(1).Select(s => s.TrimStart('-').Trim()).ToList();
            argumentGroups.Add(name, values);
        }

        return (fieldPart[0], width, height, argumentGroups);                                      
    }
}

