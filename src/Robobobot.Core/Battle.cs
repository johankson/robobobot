using System.Diagnostics.Metrics;
using Robobobot.Core;
namespace Robobobot.Server.Services;

public class Battle
{
    private readonly List<Player> players = new();
    private readonly IIdGenerator idGenerator = new IdGenerator();
    private readonly BattleRenderer renderer;

    public Battle()
    {
        renderer = new BattleRenderer(this);
    }

    public BattleType Type { get; set; } = BattleType.Regular;
    public string BattleToken { get; set; } = string.Empty;
    
    public DateTime StartTime { get;  } = DateTime.Now;

    public TimeSpan Duration => DateTime.Now - StartTime;

    public int FieldWidth { get; init; } = 20;
    public int FieldHeight { get; init; } = 20;

    public IReadOnlyList<Player> Players => players;

    public Player AddPlayer(PlayerType playerType, string name)
    {
        var player = new Player()
        {
            Token = idGenerator.Generate(),
            Type = playerType,
            Name = name 
        };

        players.Add(player);
        return player;
    }

    public string RenderPlayerVisual(string playerToken)
    {
        var player = players.FirstOrDefault(e => e.Token == playerToken);
        if (player is null)
        {
            return string.Empty;
        }

        // This should render only the visible field
        return renderer.RenderVisualBattlefieldFromCoordinate(player.Location);
    }
}
public enum PlayerType
{
    ServerBot,
    RemoteBot
}