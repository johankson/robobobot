using System.Numerics;
using System.Runtime.Intrinsics.X86;
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

        sb.AppendLine(RenderBattleField(renderWide: true, renderPlayers: true));

        return sb.ToString();
    }

    public string RenderBattleField(bool renderPlayers = false, bool renderWide = false)
    {
        var sb = new StringBuilder();
        
        for (var row = 0; row < battle.BattleField.Width; row++)
        {
            for (var col = 0; col < battle.BattleField.Height; col++)
            {
                switch (renderPlayers)
                {
                    case true when battle.Players.Any(p => p.Location.Is(col, row)):
                        sb.Append('X');
                        break;
                    default:
                        sb.Append(battle.BattleField.GetCell(col, row).GetCharType());
                        break;
                }

                if (renderWide) sb.Append(' ');
            }
            sb.AppendLine("");
        }
        
        return sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
    }

    public string RenderVisualBattlefieldPlayer(Player player)
    {
        var sb = new StringBuilder();
        for (var row = 0; row < battle.BattleField.Width; row++)
        {
            for (var col = 0; col < battle.BattleField.Height; col++)
            {
                if (col == player.Location.X && row == player.Location.Y)
                    sb.Append('X');
                else
                {
                    var c = IsCellVisibleForPlayer(player, col, row) ? battle.BattleField.GetCell(col, row).GetCharType() : ' ';
                    sb.Append(c);
                }
            }
            sb.AppendLine("");
        }
        return sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
    }
    private bool IsCellVisibleForPlayer(Player player, int x, int y)
    {
        // Naive first implementation
        var v1 = new Vector2(player.Location.X + 0.5f, player.Location.Y + 0.5f);
        var tx = x < player.Location.X ? 1f : 0f;
        var ty = y < player.Location.Y ? 1f : 0f;

        if (x == player.Location.X) tx = 0.5f;
        if (y == player.Location.Y) ty = 0.5f;
        
        var v2 = new Vector2(x + tx, y + ty);
        var diff = (v2 - v1);
        var stepDirection = Vector2.Normalize(diff) / 2f;

        var steps = diff.Length() * 2f;
        var cursor = v1;

        for (var step = 0; step < steps; step++)
        {
            cursor += stepDirection;

            var dx = (int)(cursor.X);
            var dy = (int)(cursor.Y);

            if (!battle.BattleField.GetCell(dx, dy).IsSeeThrough())
            {
                if (dx == x && dy == y)
                {
                    // It's the last cell, so we do see this one
                    return true;
                }
                
                return false;
            }
        }

        return true;

    }
}