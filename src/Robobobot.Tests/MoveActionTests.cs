using System.Threading.Tasks;
using FluentAssertions;
using Robobobot.Core.Actions;
using Xunit;

namespace Robobobot.Tests;

public class MoveActionTests
{
    [Fact]
    public async Task ShouldMoveTankOneStepNorth()
    {
        // Arrange
        var (battle, player, _) = await TestUtils.CreateSandboxWithPredefinedLevel();
        var moveAction = new MoveAction(player, MoveDirection.North);

        // Act
        var result = (await battle.EnqueueAndWait(moveAction)).Result as MoveExecutionResult;

        // Assert
        player.Location.Y.Should().Be(9);
        result.Should().NotBeNull();
        result!.NewLocation?.Y.Should().Be(9);
    }
}