namespace Robobobot.Server.BackgroundServices;

public interface IFpsController
{
    void Pause();
    void Resume();
    FpsControllerState State { get; }
    int Fps { get; set; }
}