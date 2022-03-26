namespace Robobobot.Server.Services;

public class BattleService
{
    private readonly IIdGenerator idGenerator;

    private readonly Dictionary<string, Battle> activeBattles = new();

    public BattleService()
    {
        this.idGenerator = new IdGenerator();
    }

    public BattleService(IIdGenerator idGenerator)
    {
        this.idGenerator = idGenerator;
    }
    
    public (Battle, Player) CreateSandboxBattle(string playerName, int numberOfBots)
    {
        var id = idGenerator.Generate();

        var battle = new Battle()
        {
            Id = id,
            Type = BattleType.Sandbox
        };

        var player = battle.AddPlayer(PlayerType.RemoteBot, playerName);

        for (var i = 0; i < numberOfBots; i++)
        {
            battle.AddPlayer(PlayerType.ServerBot, $"Server Bot #{i + 1}");
        }
        
        activeBattles.Add(id, battle);

        return (battle, player);
    }
    
    public Battle? Get(string battleId) => !activeBattles.ContainsKey(battleId) ? null : activeBattles[battleId];
}
public enum BattleType
{
    Sandbox,
    Regular
}