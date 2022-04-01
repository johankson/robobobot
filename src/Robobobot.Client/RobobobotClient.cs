using System.Net.Http.Headers;
using System.Net.Http.Json;
using Robobobot.Client.Extensions;
using Robobobot.Core.Models;
namespace Robobobot.Client;

/// <summary>
/// A simple .net client for Robobobot
/// </summary>
/// <remarks>
/// Download the nuget (when available)
/// </remarks>
public class RobobobotClient
{
    private readonly HttpClient httpClient;
    private const string JoinSandboxUri = "/api/Battle/join-sandbox";
    
    public RobobobotClient(string serverAddress)
    {
        httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(serverAddress);
        httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
    }

    public async Task<JoinResponse> CreateSandboxGame(string name, int numberOfBots)
    {
        var request = new JoinSandboxRequest(name, numberOfBots).ToHttpContent();
        var result = await httpClient.PostAsync(JoinSandboxUri, request);
        return await result.Content.ReadFromJsonAsync<JoinResponse>() ?? throw new InvalidOperationException();
    }
}