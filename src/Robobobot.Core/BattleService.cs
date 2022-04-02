using Robobobot.Core.Actions;
using Robobobot.Core.Models;
using Robobobot.Server.BackgroundServices;
using Robobobot.Server.Services;
namespace Robobobot.Core;

public class BattleService
{
  //  private readonly IFpsController fpsController;
    private readonly IIdGenerator idGenerator;
    private readonly Dictionary<string, Battle> activeBattles = new();

    public BattleService()
    {
        //this.fpsController = fpsController;
        this.idGenerator = new IdGenerator();
    }

    public async Task Update()
    {
        foreach (var battle in activeBattles)
        {
            await battle.Value.Update();
        }
    }
    
    public (Battle, Player) CreateSandboxBattle(string playerName, int numberOfBots = 3, BattleFieldOptions? options = null)
    {
        var id = idGenerator.Generate();

        var battle = new Battle()
        {
            BattleToken = id,
            Type = BattleType.Sandbox
        };

        if (!string.IsNullOrWhiteSpace(options?.Predefined))
        {
            battle.UsePredefinedBattleField(options.Predefined);
        }

        var player = battle.AddPlayer(PlayerType.RemoteBot, playerName);
        
        // todo - Arbitrary start position - this should be controlled by the map somehow.
        player.Location = new Location(10, 10);

        for (var i = 0; i < numberOfBots; i++)
        {
            battle.AddPlayer(PlayerType.ServerBot, $"Server Bot #{i + 1}");
        }
        
        activeBattles.Add(id, battle);

        return (battle, player);
    }
    
    public Battle? Get(string battleId) => !activeBattles.ContainsKey(battleId) ? null : activeBattles[battleId];

    /// <summary>
    /// Requests a lock to execute an action.
    /// </summary>
    /// <param name="playerToken">The player token.</param>
    /// <returns>true if a lock could be acquired, otherwise false.</returns>
    /// <remarks>
    /// Everytime a player needs to execute an action, a lock needs to be acquired to
    /// block the player from running more than one action at the time.
    /// </remarks>
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
    
    public Player? GetPlayerByToken(string playerToken)
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
}