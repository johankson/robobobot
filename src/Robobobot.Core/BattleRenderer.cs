using System.Text;
namespace Robobobot.Server.Services;

public class BattleRenderer
{
    private readonly Battle battle;
    
    public BattleRenderer(Battle battle)
    {
        this.battle = battle;
    }

    public string RenderAsText()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Mode: {battle.Type}");
        sb.AppendLine($"Start time: {battle.StartTime:yyyy-MM-dd HH\\:mm\\:ss}");
        sb.AppendLine($"The battle has raged for {battle.Duration.TotalMinutes:0} minutes and {battle.Duration.Seconds:0} seconds");
        sb.AppendLine($"Number of players: {battle.Players.Count}");
        sb.AppendLine("");
        sb.AppendLine("Players:");
        foreach (var player in battle.Players)
        {
            sb.AppendLine($"  🚗 {player.Name} - {player.Type}");
        }
       
        sb.AppendLine("");
        sb.AppendLine("The battlefield:");

        for (var row = 0; row < battle.FieldWidth; row++)
        {
            for (var col = 0; col < battle.FieldHeight; col++)
            {
                sb.Append(".");
            }
            sb.AppendLine("");
        }

        return sb.ToString();
    }
}