using System.Collections.Concurrent;
using Robobobot.Core.Models;
using Robobobot.Server.Services;

namespace Robobobot.Core;

public class BattleService
{
    private readonly IIdGenerator idGenerator;
    private readonly ConcurrentDictionary<string, Battle> activeBattles = new();

    public BattleService()
    {
        idGenerator = new IdGenerator();
        ServerLog = new InMemoryLog();
    }
    public bool HasNoActiveBattles => !activeBattles.Any();
    
    public ILog ServerLog { get; }

    public async Task Update()
    {
        foreach (var battle in activeBattles)
        {
            await battle.Value.Update();
        }

        var staleBattles = activeBattles.Where(battle => battle.Value.IsStale).ToList();
        staleBattles.ForEach(item => UnloadBattle(item.Key));
    }

    private void UnloadBattle(string battleToken)
    {
        activeBattles.Remove(battleToken, out _);
        ServerLog.Log($"Unloaded battle with id '{battleToken}");
    }
    
    public (Battle, Player) CreateSandboxBattle(string playerName, BattleFieldOptions? battleFieldOptions = null, SandboxOptions? sandboxOptions = null)
    {
        ServerLog.Log($"Creating Sandbox Battle for player {playerName}");

        try
        {
            var id = idGenerator.Generate();
            sandboxOptions ??= new SandboxOptions();

            var battle = new Battle()
            {
                BattleToken = id,
                Type = BattleType.Sandbox
            };

            if (!string.IsNullOrWhiteSpace(battleFieldOptions?.Predefined))
            {
                battle.UsePredefinedBattleField(battleFieldOptions.Predefined);
            }
            else
            {
                // For the time being, just add the default start positions along the edge of the map (8 of them)
                battle.BattleField.AddDefaultStartLocations();
            }

            var player = battle.AddPlayer(PlayerType.RemoteBot, playerName);
            
            player.Location = sandboxOptions.PlayerStartPosition ?? battle.BattleField.GetNextStartPosition(battle.Settings.RandomizeStartPositionAssignment);

            for (var i = 0; i < sandboxOptions.NumberOfBots; i++)
            {
                var enemy =battle.AddPlayer(PlayerType.ServerBot, $"Server Bot #{i + 1}");
                enemy.Location = battle.BattleField.GetNextStartPosition(battle.Settings.RandomizeStartPositionAssignment);
            }

            var couldAdd = activeBattles.TryAdd(id, battle);
            if (!couldAdd)
            {
                throw new Exception("Failed to add the battle to the internal dictionary");
            }
            
            ServerLog.Log($"Creating Sandbox Battle Complete. BattleId: '{battle.BattleToken}'");

            return (battle, player);
        }
        catch (Exception e)
        {
            ServerLog.Log($"Failed to create Sandbox Battle. Exception message: '{e.Message}'");
            throw;
        }
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

    public (Battle?, Player?) GetBattleAndPlayerByPlayerToken(string playerToken)
    {
        var result = activeBattles.SelectMany(battle => battle.Value.Players
            .Where(player => player.Token == playerToken), (b, p) => (b.Value, p)).FirstOrDefault();
        return result;
    }
}