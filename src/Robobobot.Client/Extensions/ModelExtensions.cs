using System.Text;
using System.Text.Json;
using Robobobot.Core.Models;
namespace Robobobot.Client.Extensions;

public static class ModelExtensions
{
    public static StringContent ToHttpContent(this JoinSandboxRequest request) => new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
}