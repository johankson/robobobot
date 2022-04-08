using Robobobot.Core.Models;
using Robobobot.Server.Services;
namespace Robobobot.Core;

public class Player
{
    public string Token { get; set; } = string.Empty;
    public string Name { get; set; } = "Player";
    public PlayerType Type { get; set; } = PlayerType.RemoteBot;
    public Location Location { get; set; } = new(0, 0);
    public int TurretDegree { get; set; } = 0;
    public int TankDegree { get; set; } = 0;

    public bool RequestActionLock()
    {
        lock (pendingActionLock)
        {
            if (hasPendingAction)
            {
                return false;
            }

            hasPendingAction = true;
            return true;
        }
    }

    public void ReleaseActionLock()
    {
        lock (pendingActionLock)
        {
            hasPendingAction = false;
        }
    }

    private bool hasPendingAction;
    private readonly object pendingActionLock = new();
    
    
}