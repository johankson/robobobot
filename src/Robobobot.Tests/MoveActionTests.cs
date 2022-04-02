using System.Threading.Tasks;
using FluentAssertions;
using Robobobot.Core;
using Robobobot.Core.Actions;
using Xunit;
using Xunit.Abstractions;

namespace Robobobot.Tests;

public class MoveActionTests
{
    private readonly ITestOutputHelper log;
    
    public MoveActionTests(ITestOutputHelper log)
    {
        this.log = log;
    }
    
    [Fact]
    public async Task ShouldMoveTankOneStepNorth()
    {
        // Arrange
        var (battle, player, _) = await TestUtils.CreateSandboxWithPredefinedLevel();
        var moveAction = new MoveAction(player, MoveDirection.North);
        log.WriteLine("Pre move map:");
        log.WriteLine(battle.Renderer.RenderBattleField(true, true));

        // Act
        var result = (await moveAction.Execute(battle)) as MoveExecutionResult; 
        log.WriteLine("Post move map:");
        log.WriteLine(battle.Renderer.RenderBattleField(true, true));

        // Assert
        player.Location.Y.Should().Be(9);
        result.Should().NotBeNull();
        result!.FinalLocation?.Y.Should().Be(9);
        result.Success.Should().BeTrue();
    }
    
    [Fact]
    public async Task ShouldNotMoveTankOneStepNorthDueToMountains()
    {
        // Arrange
        var (battle, player, _) = await TestUtils.CreateSandboxWithPredefinedLevel();
        player.Location = new Location(10, 9); // Just below the mountains
        var moveAction = new MoveAction(player, MoveDirection.North);
        log.WriteLine("Pre move map:");
        log.WriteLine(battle.Renderer.RenderBattleField(true, true));

        // Act
        var result = (await moveAction.Execute(battle)) as MoveExecutionResult; 
        log.WriteLine("Post move map:");
        log.WriteLine(battle.Renderer.RenderBattleField(true, true));

        // Assert
        player.Location.Y.Should().Be(9);
        result.Should().NotBeNull();
        result!.FinalLocation?.Y.Should().Be(9);
        result.Success.Should().BeFalse();
    }
}