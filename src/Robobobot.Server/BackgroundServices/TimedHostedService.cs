namespace Robobobot.Server.BackgroundServices;

public class TimedHostedService : IHostedService, IDisposable, IFpsController
{
    private int frameNumber;
    private readonly ILogger<TimedHostedService> logger;
    private Timer timer = null!;
    private int fps;
    
    public FpsControllerState State { get; private set; }

    public TimedHostedService(ILogger<TimedHostedService> logger)
    {
        this.logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Timed Hosted Service running.");

        timer = new Timer(DoWork, null, Timeout.InfiniteTimeSpan, TimeSpan.Zero);
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
    
    private void DoWork(object? state)
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
            logger.LogInformation("Executing frame #{Count}", count);
        }
        finally {
        {
            working = false;
        }}
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Timed Hosted Service is stopping.");
        SetTimer(Timeout.InfiniteTimeSpan, TimeSpan.Zero);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
    
    private void SetFps(int value)
    {
        if (value < 1) throw new ArgumentException("Fps cannot be set under 1, just pause it instead");
        
        var duration = 1000d / value;
        
        fps = value;
        timer?.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(duration));
        State = FpsControllerState.Running;
    }

    public void Pause()
    {
        SetTimer(Timeout.InfiniteTimeSpan, TimeSpan.Zero);
        State = FpsControllerState.Paused;
    }
    
    public void Resume()
    {
        SetFps(fps);
    }
    
    public int Fps
    {
        get => fps;
        set => SetFps(value);
    }
}