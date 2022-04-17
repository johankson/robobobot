using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Robobobot.Core;
using Robobobot.Core.Actions;
using Robobobot.Core.Models;
using Xunit;
using Xunit.Abstractions;

namespace Robobobot.Tests;

public class GetVisualActionTests
{
    private readonly ITestOutputHelper log;

    public GetVisualActionTests(ITestOutputHelper log)
    {
        this.log = log;
    }
    
    [Theory]
    [InlineData("Actions/GetVisual/1_Level.txt", "Actions/GetVisual/1_Rendered.txt")]
    public async Task ShouldMatchFixtures(string levelPath, string renderedPath)
    {
        // Arrange
        var level = await File.ReadAllTextAsync(Path.Combine("Fixtures", levelPath));
        var rendered = await File.ReadAllTextAsync(Path.Combine("Fixtures", renderedPath));
        
        var service = new BattleService();
        var options = new BattleFieldOptions(Predefined: level);
        var (battle, player) = service.CreateSandboxBattle("test", battleFieldOptions: options);
        player.Location = new Location(10, 10);
        battle.Settings.RandomizeStartPositionAssignment = false;

        // Act
        var action = new GetVisualAction(player);
        var result = await action.Execute(battle);
        var renderedLevel = (result as GetVisualExecutionResult)?.BattleField;

        // Assert
        renderedLevel.Should().NotBeNull();
        renderedLevel.Should().BeEquivalentTo(rendered);
    }
    
    [Fact]
    public async Task ShouldGetVisualNearEdge()
    {
        // Arrange
        var level = await File.ReadAllTextAsync(Path.Combine("Fixtures", "Actions/GetVisual/1_Level.txt"));
        
        var service = new BattleService();
        var options = new BattleFieldOptions(Predefined: level);
        var (battle, player) = service.CreateSandboxBattle("test", battleFieldOptions: options);
        player.Location = new Location(19, 10);
        battle.Settings.RandomizeStartPositionAssignment = false;

        // Act
        var action = new GetVisualAction(player);
        var result = await action.Execute(battle);
        var renderedLevel = (result as GetVisualExecutionResult)?.BattleField;
        log.WriteLine(renderedLevel);
        
        // Assert
        renderedLevel.Should().NotBeNull();
    }
}