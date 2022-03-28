using Robobobot.Server.Services;
namespace Robobobot.Core;

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
            BattleToken = id,
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

    public bool RequestActionLock(string playerToken)
    {
        var player = GetPlayerByToken(playerToken);
        return player?.RequestActionLock() ?? false;
    }
   
    public void ReleaseActionLock(string playerToken)
    {
        var player = GetPlayerByToken(playerToken);
        player?.ReleaseActionLock();
    }
    
    private Player? GetPlayerByToken(string playerToken)
    {
        // Concept to get the player with a given token.
        // A cache might be better here.
        var player = activeBattles.SelectMany(battle => battle.Value.Players
            .Where(player => player.Token == playerToken), (_, p) => p).FirstOrDefault();
        
        return player;
    }

    public Battle? GetBattleByPlayerToken(string playerToken)
    {
        // Concept to get the battle with a given player token.
        // A cache might be better here.
        var battle = activeBattles.SelectMany(battle => battle.Value.Players
            .Where(player => player.Token == playerToken), (b, _) => b.Value).FirstOrDefault();

        return battle;
    }

    // Refactor this into some command based plug in thingy
    private string GetPlayerVisual(string playerToken)
    {
        var battle = GetBattleByPlayerToken(playerToken);
        return battle?.RenderPlayerVisual(playerToken) ?? string.Empty;
    }
}