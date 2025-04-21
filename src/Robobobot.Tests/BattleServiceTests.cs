using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Robobobot.Core;
using Robobobot.Core.Models;
using Robobobot.Core.Parsers;
using Robobobot.Server.BackgroundServices;
using Xunit;

namespace Robobobot.Tests;

public class BattleServiceTests
{
    [Fact]
    public void ShouldCreateABattleServiceWithADefaultIdGenerator()
    {
        var service = new BattleService();
        service.Should().NotBeNull();
    }

    [Fact]
    public void ShouldCreateSandboxBattle()
    {
        // Arrange
        var service = new BattleService();
        
        // Act
        var (battle, _) = service.CreateSandboxBattle("Bengt");
        
        // Assert
        battle.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldCreateSandboxBattleWithPredefinedLevel()
    {
        // Arrange
        const string levelPath = "Actions/GetVisual/1_Level.txt";
        var level = await File.ReadAllTextAsync(Path.Combine("Fixtures", levelPath));
        var service = new BattleService();
        var options = new BattleFieldOptions(Predefined: level);
        
        // Act
        var (battle, _) = service.CreateSandboxBattle("Bengt", options);
        
        // Assert
        battle.Renderer.RenderBattleField().Should().BeEquivalentTo(new BattleFieldParser().Parse(level).map);
    }

    [Fact]
    public async Task ShouldListAllBattles()
    {
        var service = new BattleService();
        service.CreateSandboxBattle("Battle 1");
        service.CreateSandboxBattle("Battle 2");
        
        var battles = service.GetAllBattles();
        battles.Should().HaveCount(2);
    }
}