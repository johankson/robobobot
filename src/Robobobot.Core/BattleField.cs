namespace Robobobot.Core;

public class BattleField
{
    public int Width { get; set; }
    public int Height { get; set; }

    public BattleField(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public static BattleField FromPreExistingBattleField(string battleField)
    {
        //throw new NotImplementedException();
        return new BattleField(20, 20);
    }
}

public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
}