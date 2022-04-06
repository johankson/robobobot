using Robobobot.Core;

namespace Robobobot.Server.BackgroundServices;

public class GameLoopHostedService : IHostedService, IDisposable, IFpsController
{
    private int frameNumber;
    private readonly ILogger<GameLoopHostedService> logger;
    private readonly BattleService battleService;
    private Timer timer = null!;
    private int fps;
    
    public FpsControllerState State { get; private set; }

    public GameLoopHostedService(BattleService battleService, ILogger<GameLoopHostedService> logger)
    {
        this.battleService = battleService;
        this.logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Game loop is running");

        timer = new Timer(ExecuteGameTick, null, Timeout.InfiniteTimeSpan, TimeSpan.Zero);
        Fps = 10;
        
        // Pause the server directly until we have a battle...
        Pause();

        return Task.CompletedTask;
    }

    private void SetTimer(TimeSpan dueTime, TimeSpan period)
    {
        timer.Change(dueTime, period);
    }

    private readonly object lockObject = new();
    private bool working;
    
    private void ExecuteGameTick(object? state)
    {
        lock (lockObject)
        {
            if (working)
            {
                logger.LogWarning("Frame dropped!");
                return;
            }

            working = true;
        }

        try
        {
            var count = Interlocked.Increment(ref frameNumber);
            logger.LogTrace("Executing frame #{Count}", count);

            if (battleService.HasNoActiveBattles && State != FpsControllerState.Running)
            {
                logger.LogInformation("Pausing game loop since there are no active battles");
                Pause();
            }
            
            battleService.Update();
        }
        finally {
        {
            working = false;
        }}
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Game loop timer is stopping");
        SetTimer(Timeout.InfiniteTimeSpan, TimeSpan.Zero);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer.Dispose();
        GC.SuppressFinalize(this);
    }
    
    private void SetFps(int value)
    {
        if (value < 1) throw new ArgumentException("Fps cannot be set under 1, just pause it instead");
        
        var duration = 1000d / value;
        
        fps = value;
        timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(duration));
        State = FpsControllerState.Running;
        
        logger.LogInformation("Fps is set to {Fps} and the server is running", fps);
    }

    public void Pause()
    {
        logger.LogTrace("Pause called, disabling game loop timer");
        SetTimer(Timeout.InfiniteTimeSpan, TimeSpan.Zero);
        State = FpsControllerState.Paused;
    }
    
    public void Resume()
    {
        logger.LogTrace("Resume called, enabling game loop timer {Fps}", fps);
        SetFps(fps);
    }
    
    public int Fps
    {
        get => fps;
        set => SetFps(value);
    }
}