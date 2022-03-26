using FluentAssertions;
using Robobobot.Server.Services;
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
        var battleToken = service.CreateSandboxBattle("Bengt", 2);
        
        // Assert
        battleToken.Should().NotBeNull();
    }
}