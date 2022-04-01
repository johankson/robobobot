namespace Robobobot.Core.Models;

public record JoinRequest(string BattleId, string PlayerToken);
public record JoinResponse(string BattleId, string PlayerToken);

public record JoinSandboxRequest(string Name, int NumberOfBots = 3);
