using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Robobobot.Client.IntegrationTests;

public class CreateSandboxGameTests
{
    [Fact]
    public async Task ShouldCreateSandboxGame()
    {
        // Arrange
        var client = new RobobobotClient("https://localhost:7297");

        // Act
        var (battleId, playerToken) = await client.CreateSandboxGame("Tester", 3);

        // Assert
        battleId.Should().NotBeNull();
        playerToken.Should().NotBeNull();
    }
}
