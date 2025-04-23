using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Robobobot.Core.Models;

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
        try
        {
            var sb = new StringBuilder();
            for (var row = 0; row < battle.BattleField.Height; row++)
            {
                for (var col = 0; col < battle.BattleField.Width; col++)
                {
                    if (player.Location.Is(col, row))
                    {
                        sb.Append('X');
                    }
                    else
                    {
                        var c = IsCellVisibleForPlayer(player, col, row) ? battle.BattleField.GetCell(col, row).GetCharType() : ' ';
                        if (c != ' ' && IsEnemyAtLocation(Location.Create(col, row), out char shortEnemyToken))
                        {
                            sb.Append(shortEnemyToken);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                }
                sb.AppendLine("");
            }
            return sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private bool IsEnemyAtLocation(Location location, out char shortToken)
    {
        shortToken = battle.Players.FirstOrDefault(p => p.Location == location)?.ShortToken ?? ' ';
        return shortToken != ' ';
    }
    
    private bool IsCellVisibleForPlayer(Player player, int x, int y)
    {
        // Naive first implementation
        var v1 = new Vector2(player.Location.X + 0.5f, player.Location.Y + 0.5f); // + 0.5f to center the player
        var tx = x < player.Location.X ? 1f : 0f;
        var ty = y < player.Location.Y ? 1f : 0f;

        if (x == player.Location.X) tx = 0.5f;
        if (y == player.Location.Y) ty = 0.5f;
        
        var v2 = new Vector2(x + tx, y + ty);
        var diff = (v2 - v1);
        var stepDirection = Vector2.Normalize(diff) / 2f;

        var steps = diff.Length() * 2f;
        var cursor = v1;

        try
        {
            for (var step = 0; step < steps; step++)
            {
                cursor += stepDirection;

                var dx = (int)(cursor.X);
                var dy = (int)(cursor.Y);

                if (dx < 0 || dx > battle.BattleField.Width - 1 || dy < 0 || dy > battle.BattleField.Height - 1)
                    continue;

                if (battle.BattleField.GetCell(dx, dy).IsSeeThrough())
                {
                    continue;
                }
                
                return dx == x && dy == y;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return true;

    }
}