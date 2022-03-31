using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Robobobot.Core;
using Robobobot.Core.Actions;
using Xunit;

namespace Robobobot.Tests;

public class GetVisualActionTests
{
    [Theory]
    [InlineData("Actions/GetVisual/1_Level.txt", "Actions/GetVisual/1_Rendered.txt")]
    public async Task ShouldMatchFixtures(string levelPath, string renderedPath)
    {
        // Arrange
        var level = await File.ReadAllTextAsync(Path.Combine("Fixtures", levelPath));
        var rendered = await File.ReadAllTextAsync(Path.Combine("Fixtures", renderedPath));
        
        var service = new BattleService();
        var (battle, player) = service.CreateSandboxBattle("test", battleField: level);
        player.Location = new Location(10, 10);

        // Act
        var action = new GetVisualAction(player);
        var result = await action.Execute(battle);
        var renderedLevel = (result as GetVisualExecutionResult)?.BattleField;

        // Assert
        renderedLevel.Should().NotBeNull();
        renderedLevel.Should().BeEquivalentTo(rendered);
    }
}