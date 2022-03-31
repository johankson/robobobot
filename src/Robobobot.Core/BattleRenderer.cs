using System.Text;

namespace Robobobot.Core;

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

        sb.AppendLine(RenderBattleField());

        return sb.ToString();
    }

    public string RenderBattleField()
    {
        var sb = new StringBuilder();
        for (var row = 0; row < battle.BattleField.Width; row++)
        {
            for (var col = 0; col < battle.BattleField.Height; col++)
            {
                sb.Append(battle.BattleField.GetCell(col, row).GetCharType()); // Logic here to view something
            }
            sb.AppendLine("");
        }
        return sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
    }

    public string RenderVisualBattlefieldPlayer(Player player)
    {
        
        
        
        return RenderBattleField();
    }
}