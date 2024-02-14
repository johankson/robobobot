using Robobobot.Core.Actions;
using Robobobot.Server.Services;
namespace Robobobot.Core;

public class Battle
{
    private readonly List<Player> players = new();
    private readonly List<Shell> shells = new();
    private readonly IIdGenerator idGenerator = new IdGenerator();
    private readonly BattleRenderer renderer;
    private BattleField battleField = new(20, 20);

    public Battle()
    {
        renderer = new BattleRenderer(this);
    }

    public BattleRenderer Renderer => renderer;

    public BattleField BattleField => battleField;

    public void GenerateBattleField(int width, int height)
    {
        // todo create a battlefield generator...
        throw new NotImplementedException();
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
        
        // Update the dangerous stuff flying around.
        lock (shells)
        {
            foreach (var shell in shells)
            {
                shell.Update(elapsedGameTime, this);
            }

            shells.RemoveAll(x => x.MarkForDeletion);
        }
        
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
        // The code to do stuff (like movement) is placed in the actual action.
        // This is a design decision to keep it as simple as possible to add new
        // actions to the game.
        foreach (var action in frameActions)
        {
            var result = await action.Execute(this);
            action.Result = result;
            action.CompleteCallback?.Invoke();
        }

  
    }
    
    /// <summary>
    /// Signals that the battle is stale.
    /// </summary>
    /// <remarks>This marks that it should be removed from the server.</remarks>
    public bool IsStale { get; set; }

    public Player? FindPlayerByToken(string playerToken) =>
        players.FirstOrDefault(player => player.Token == playerToken);
    
    public void AddShell(Shell shell)
    {
        lock (shell)
        {
            shells.Add(shell);
        }
    }
}