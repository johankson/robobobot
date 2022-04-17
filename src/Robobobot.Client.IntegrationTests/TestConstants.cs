namespace Robobobot.Client.IntegrationTests;

public static class TestConstants
{
    public const string LocalAddress = "https://localhost:7297";
    public const string RemoteAddress = "https://robobobot.azurewebsites.net";
    public const bool UseAzure = false;
    
    public static string ResolveRemoteAddress() =>
        UseAzure ? RemoteAddress : LocalAddress;
}