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
    private const string GetVisualUri = "/api/Battle/get-visual";
    private const string GetReadingsUri = "/api/Battle/get-readings";
    private const string AimUri = "/api/Battle/aim";
    private const string MoveUri = "/api/Battle/move";
    private const string FireUri = "/api/Battle/fire";
    private string playerToken = "";
    
    public RobobobotClient(string serverAddress)
    {
        httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(serverAddress);
        httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
    }

    public async Task<JoinResponse> CreateSandboxGame(string name, BattleFieldOptions? battleFieldOptions = null, SandboxOptions? sandboxOptions = null)
    {
        var request = new JoinSandboxRequest(name, battleFieldOptions, sandboxOptions).ToHttpContent();
        var result = await httpClient.PostAsync(JoinSandboxUri, request);
        var joinResponse = await result.Content.ReadFromJsonAsync<JoinResponse>() ?? throw new InvalidOperationException();

        if (result.IsSuccessStatusCode)
        {
            playerToken = joinResponse.PlayerToken;
            httpClient.DefaultRequestHeaders.Add("Token", playerToken);
        }
        
        return joinResponse;
    }

    public async Task<GetVisualExecutionResult> GetVisual()
    {
        var result = await httpClient.GetFromJsonAsync<GetVisualExecutionResult>(GetVisualUri);
        if (result == null)
        {
            throw new Exception("oops");
        }
        return result;
    }
    public async Task<MoveExecutionResult> Move(MoveDirection direction)
    {
        var result = await httpClient.GetFromJsonAsync<MoveExecutionResult>($"{MoveUri}/{direction.ToString()}");
        if (result == null)
        {
            throw new Exception("oops");
        }
        return result;
    }
    public async Task<GetReadingsResult> GetReadings()
    {
        var result = await httpClient.GetFromJsonAsync<GetReadingsResult>($"{GetReadingsUri}");
        if (result == null)
        {
            throw new Exception("oops");
        }
        return result;
    }
    public async Task<AimActionResult> Aim(int degrees)
    {
        var result = await httpClient.GetFromJsonAsync<AimActionResult>($"{AimUri}/{degrees}");
        if (result == null)
        {
            throw new Exception("oops");
        }
        return result;
    }
    
    public async Task<AimActionResult> Fire()
    {
        var result = await httpClient.GetFromJsonAsync<AimActionResult>($"{FireUri}");
        if (result == null)
        {
            throw new Exception("oops");
        }
        return result;
    }
}