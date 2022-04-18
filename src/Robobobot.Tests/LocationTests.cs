using System.Numerics;
using FluentAssertions;
using Robobobot.Core.Models;
using Xunit;
namespace Robobobot.Tests;

public class LocationTests
{
    [Theory]
    [InlineData(0.3f, 1.1f, 0, 1)]
    [InlineData(1.9f, 1.1f, 1, 1)]
    [InlineData(1.9f, 0.9f, 1, 0)]
    public void ShouldTruncateLocationsCorrectlyWhenComparingToVectorOfFloats(float x, float y, int expectedX, int expectedY)
    {
        var vector = new Vector2(x, y);
        var location = Location.Create(expectedX, expectedY);
        
        // Act
        location.Is(vector).Should().BeTrue();
    }
}