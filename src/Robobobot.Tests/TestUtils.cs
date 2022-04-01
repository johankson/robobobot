using System.IO;
using System.Threading.Tasks;
using Robobobot.Core;
using Robobobot.Core.Models;

namespace Robobobot.Tests;

public static class TestUtils
{
    public static async Task<(Battle, Player, BattleService)> CreateSandboxWithPredefinedLevel()
    {
        const string levelPath = "Actions/GetVisual/1_Level.txt";
        var level = await File.ReadAllTextAsync(Path.Combine("Fixtures", levelPath));
        var service = new BattleService();
        var options = new BattleFieldOptions(Predefined: level);
        var (battle, player) = service.CreateSandboxBattle("Bengt", 2, options);

        return (battle, player, service);
    }
}