using System.Threading.Tasks;
using FluentAssertions;
using Robobobot.Core.Models;
using Xunit;

namespace Robobobot.Client.IntegrationTests;

public class CreateSandboxGameTests
{
    [Fact]
    public async Task ShouldCreateSandboxGame()
    {
        // Arrange
        var client = new RobobobotClient(TestConstants.ResolveRemoteAddress());

        // Act
        var (battleId, playerToken, playerName) = await client.CreateSandboxGame("Tester");

        // Assert
        battleId.Should().NotBeNull();
        playerToken.Should().NotBeNull();
        playerName.Should().Be("Tester");
    }
    
    [Fact]
    public async Task ShouldCreateVisualNearEdge()
    {
        // Arrange
        var client = new RobobobotClient(TestConstants.ResolveRemoteAddress());

        // Act
        var (battleId, playerToken, playerName) = await client.CreateSandboxGame("Tester", sandboxOptions: new SandboxOptions(PlayerStartPosition: Location.Create(19,9)));
        var visual = await client.GetVisual();
        
        // Assert
        battleId.Should().NotBeNull();
        playerToken.Should().NotBeNull();
        playerName.Should().Be("Tester");
    }
}
