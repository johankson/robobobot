namespace Robobobot.Server.Services;

public class Battle
{
    private readonly List<Player> players = new();
    private readonly IIdGenerator idGenerator = new IdGenerator();

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
}

public class Player
{
    public string Token { get; set; } = string.Empty;
    public string Name { get; set; } = "Player";
    public PlayerType Type { get; set; } = PlayerType.RemoteBot;
}

public enum PlayerType
{
    ServerBot,
    RemoteBot
}