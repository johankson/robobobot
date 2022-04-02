using Robobobot.Core.Actions;
using Robobobot.Server.BackgroundServices;
using Robobobot.Server.Services;
namespace Robobobot.Core;

public class Battle
{
    private readonly List<Player> players = new();
    private readonly IIdGenerator idGenerator = new IdGenerator();
    private readonly BattleRenderer renderer;
    private BattleField battleField = new BattleField(20, 20);

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
        var player = new Player()
        {
            Token = idGenerator.Generate(),
            Type = playerType,
            Name = name 
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
    
    public async Task Update()
    {
        // Copy the actions in the buffer
        var frameActions = new List<ActionBase>();
        lock (nextFrameActions)
        {
            frameActions.InsertRange(0, nextFrameActions);
            nextFrameActions.Clear();
        }
        
        // Execute the actions
        // Todo Sort by type (movement first, the aim and so on)
        foreach (var action in frameActions)
        {
            var result = await action.Execute(this);
            action.Result = result;
            action.CompleteCallback?.Invoke();
        }
    }
}