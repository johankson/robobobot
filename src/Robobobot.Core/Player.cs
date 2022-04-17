using Robobobot.Core.Models;
using Robobobot.Server.Services;
namespace Robobobot.Core;

public class Player
{
    public string Token { get; set; } = string.Empty;
    public string Name { get; set; } = "Player";
    public PlayerType Type { get; set; } = PlayerType.RemoteBot;
    public Location Location { get; set; } = new(0, 0);
    
    /// <summary>
    /// The current angle of the turret relative to the tanks heading, CW, 0 is the angle that the tank is facing. 
    /// </summary>
    public int TurretDegree { get; set; } = 0;
    
    /// <summary>
    /// The current heading of the tank, in degrees. CW, 0 is up.
    /// </summary>
    public int TankHeading { get; set; } = 0;
    
    /// <summary>
    /// 
    /// </summary>
    public char ShortToken { get; set; }

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