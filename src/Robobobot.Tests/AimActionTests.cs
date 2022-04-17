using System.Threading.Tasks;
using FluentAssertions;
using Robobobot.Core;
using Robobobot.Core.Actions;
using Robobobot.Core.Models;
using Xunit;
namespace Robobobot.Tests;

public class AimActionTests
{
    [Fact]
    public async Task ShouldAimTowardsASpecificHeading()
    {
        // Arrange
        var service = new BattleService();
        var (battle, player) = service.CreateSandboxBattle("test");
        var action = new AimAction(player, 45);

        // Act
        var result = await action.Execute(battle) as AimActionResult;

        // Assert
        result.Should().NotBeNull();
        result!.FinalAngle.Should().Be(45);
    }
    
    [Fact]
    public async Task ShouldHandleNegativeAimUnderflow()
    {
        // Arrange
        var service = new BattleService();
        var (battle, player) = service.CreateSandboxBattle("test");
        var action = new AimAction(player, -45);

        // Act
        var result = await action.Execute(battle) as AimActionResult;

        // Assert
        result.Should().NotBeNull();
        result!.FinalAngle.Should().Be(315);
    }
    
    [Theory]
    [InlineData(-45, 450)]
    [InlineData(45, 450)]
    [InlineData(370, 100)]
    [InlineData(-370, 100)]
    public async Task ShouldBePositiveDurationTurretMovement(int deltaAngle, int expectedDuration)
    {
        // Arrange
        var service = new BattleService();
        var (battle, player) = service.CreateSandboxBattle("test");
        battle.Settings.ExecutionDurations.AimDurationPerDegree = 10;
        var action = new AimAction(player, deltaAngle);

        // Act
        var result = await action.Execute(battle) as AimActionResult;

        // Assert
        result.Should().NotBeNull();
        result!.ExecutionDuration.Should().Be(expectedDuration);
    }
}