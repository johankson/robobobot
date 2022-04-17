using System.Threading.Tasks;
using FluentAssertions;
using Robobobot.Core;
using Robobobot.Core.Actions;
using Robobobot.Core.Models;
using Xunit;
namespace Robobobot.Tests;

public class GetReadingsActionTests
{
    [Fact]
    public async Task ShouldGetBaseReadingsAtStartOfGame()
    {
        // Arrange
        var service = new BattleService();
        var options = new BattleFieldOptions();
        var (battle, player) = service.CreateSandboxBattle("test", battleFieldOptions: options);
        player.Location = new Location(10, 10);
        battle.Settings.RandomizeStartPositionAssignment = false;

        // Act
        var action = new GetReadingsAction(player);
        var result = await action.Execute(battle) as GetReadingsResult;

        // Assert
        result.Should().NotBeNull();
        result!.Values.ContainsKey(GetReadingsAction.TurretAngleMoniker).Should().BeTrue();
        result!.Values[GetReadingsAction.TurretAngleMoniker].Should().Be("0");
        result!.Values[GetReadingsAction.TankHeadingMoniker].Should().Be("0");
    }
    
    [Fact]
    public async Task ShouldGetAimReadingAfterAimingGame()
    {
        // Arrange
        var service = new BattleService();
        var options = new BattleFieldOptions();
        var (battle, player) = service.CreateSandboxBattle("test", battleFieldOptions: options);
        player.Location = new Location(10, 10);
        battle.Settings.RandomizeStartPositionAssignment = false;
        var aimAction = new AimAction(player, 45);
        await aimAction.Execute(battle);

        // Act
        var action = new GetReadingsAction(player);
        var result = await action.Execute(battle) as GetReadingsResult;

        // Assert
        result.Should().NotBeNull();
        result!.Values.ContainsKey(GetReadingsAction.TurretAngleMoniker).Should().BeTrue();
        result!.Values[GetReadingsAction.TurretAngleMoniker].Should().Be("45");
        result!.Values[GetReadingsAction.TankHeadingMoniker].Should().Be("0");
    }
}