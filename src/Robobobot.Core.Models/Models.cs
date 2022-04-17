namespace Robobobot.Core.Models;


public record JoinRequest(string BattleId, string PlayerToken);
public record JoinResponse(string BattleToken, string PlayerToken, string PlayerName);

/// <summary>
/// Request a sandbox battle to be created.
/// </summary>
/// <param name="Name">The name of the player.</param>
/// <param name="BattleFieldOptions">Optional battle field generation parameters.</param>
/// <param name="SandboxOptions">Options for specific sandbox settings.</param>
public record JoinSandboxRequest(string Name, BattleFieldOptions? BattleFieldOptions = null, SandboxOptions? SandboxOptions = null);

/// <summary>
/// Options for creating a battlefield.
/// </summary>
/// <param name="Seed">The seed to use for the randomizer. Does not effect width & height.</param>
/// <param name="Width">The width of the battlefield.</param>
/// <param name="Height">The height of the battlefield.</param>
/// <param name="Predefined">A predefined custom level, other Seed, Width and Height are ignored.</param>
public record BattleFieldOptions(string Seed = "", int Width = 100, int Height = 100, string Predefined = "");


/// <summary>
/// Options controlling how the sandbox is created.
/// </summary>
/// <param name="SpeedModifier"></param>
public record SandboxOptions(float SpeedModifier = 1f, int NumberOfBots = 3, Location? PlayerStartPosition = null);