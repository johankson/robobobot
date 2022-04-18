using Robobobot.Core.Actions;
using Robobobot.Server.Services;
namespace Robobobot.Core;

public class Battle
{
    private readonly List<Shell> shells = new();
    private readonly List<Player> players = [];
    private readonly IIdGenerator idGenerator = new IdGenerator();
    private readonly BattleRenderer renderer;
    private BattleField battleField = new(20, 20);

    public Battle()
    {
        renderer = new BattleRenderer(this);
    }

    public BattleRenderer Renderer => renderer;

    public BattleField BattleField => battleField;

    public void GenerateBattleField(string seed, int width, int height)
    {
        if (width < 10 || height < 10 || width > 1000 || height > 1000)
        {
            throw new InvalidMapSizeException("Width and height must be between 10 and 1000");
        }

        battleField = new BattleField(width, height);
        
        if (string.IsNullOrWhiteSpace(seed))
        {
            // Randomize the seed...
            var random = new Random();
            seed = $"{random.Next(0, 10000)}-{DateTime.Now.Ticks}";
        }
        
        // Create a numeric seed from the string
        // DO NOT use seed.GetHashCode() since it will be different on different platforms
        // and randomized by the run time between different executions of the server.
        var hash = System.HashCode.Combine(seed);
        
        // Use a noise algorithm to generate the map
        SimplexNoise.Noise.Seed = hash;
        var scale = 0.04f;
        var noise = SimplexNoise.Noise.Calc2D(width, height, scale);
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var value = noise[x, y];
                var cell = battleField.GetCell(x, y);

                cell.Type = value switch
                {
                    < 30f => CellType.Swamp,
                    < 150f => CellType.Land,
                    < 230f => CellType.Forrest,
                    < 400f => CellType.Mountain,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        
        // For the time being, just add the default start positions along the edge of the map (8 of them)
        battleField.AddDefaultStartLocations();
        
        // BIG TODO:
        // The start position should be randomized but also not put the player
        // in a swamp or mountain, a cell that is not walkable or to close to another player.
        // The start position must also be playable so we are not surrounded by unwalkable cells.
        // And on top of that, it must be repeatable from the same seed.
    }

    public void UsePredefinedBattleField(string preDefinedBattleField)
    {
        battleField = BattleField.FromPreExistingMap(preDefinedBattleField);
    }

    public BattleType Type { get; set; } = BattleType.Regular;
    public string BattleToken { get; set; } = string.Empty;

    public BattleSettings Settings { get; set; } = BattleSettings.Default;
    
    public DateTime StartTime { get;  } = DateTime.Now;

    public TimeSpan Duration => DateTime.Now - StartTime;

    public IReadOnlyList<Player> Players => players;

    public Player AddPlayer(PlayerType playerType, string name)
    {
        if (players.Count == 9)
        {
            throw new Exception("We cant handle more than nine players since we use a digit as the short token. Letters will be implemented later on if needed.");
        }
        
        var player = new Player()
        {
            Token = idGenerator.Generate(),
            Type = playerType,
            Name = name,
            ShortToken = (players.Count+1).ToString().ToCharArray().First()
        };

        players.Add(player);
        return player;
    }

    public async Task<T> EnqueueAndWait<T>(T action) where T : ActionBase
    {
        lock (nextFrameActions)
        {
            nextFrameActions.Add(action);
        }

        using var sph = new SemaphoreSlim(0, 1);
        
        action.OnComplete(() =>
        {
            // ReSharper disable once AccessToDisposedClosure
            sph?.Release();
        });

        var t = sph.WaitAsync();
            
        if (await Task.WhenAny(t, Task.Delay(10000)) == t)
        {
            return action;
        }

        throw new TimeoutException();
    }

    private readonly List<ActionBase> nextFrameActions = new();
    private DateTime lastReceivedActionTimeStamp = DateTime.Now;
    private DateTime lastUpdate = DateTime.Now;

    public async Task Update()
    {
        // Calc the elapsed time for this battle, diff = from beginning of last update to the beginning of this one.
        var elapsedGameTime = (float) (DateTime.Now - lastUpdate).TotalMilliseconds;
        lastUpdate = DateTime.Now;
        
        // Copy the actions in the buffer
        var frameActions = new List<ActionBase>();
        lock (nextFrameActions)
        {
            frameActions.InsertRange(0, nextFrameActions);
            nextFrameActions.Clear();
        }

        if (frameActions.Any())
        {
            lastReceivedActionTimeStamp = DateTime.Now;
        }
        else
        {
            var diff = DateTime.Now - lastReceivedActionTimeStamp;

            if (diff.Minutes > Settings.StaleTimeoutInMinutes)
            {
                IsStale = true;
            }
            return;
        }

        // Execute the actions
        // Todo Sort by type (movement first, the aim and so on)
        foreach (var action in frameActions)
        {
            var result = await action.Execute(this);
            action.Result = result;
            action.CompleteCallback?.Invoke();
        }

        // Update the dangerous stuff flying around.
        foreach (var shell in shells)
        {
            shell.Update(elapsedGameTime, this);
        }

        shells.RemoveAll(x => x.MarkForDeletion);
    }
    
    /// <summary>
    /// Signals that the battle is stale.
    /// </summary>
    /// <remarks>This marks that it should be removed from the server.</remarks>
    public bool IsStale { get; set; }

    public Player? FindPlayerByToken(string playerToken) =>
        players.FirstOrDefault(player => player.Token == playerToken);
}