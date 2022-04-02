namespace Robobobot.Core.Extensions;

public static class CellExtensions
{
    public  delegate void CellAction(int x, int y, Cell cell);

    public static void ForEach(this Cell[,] cells, CellAction action)
    {
        for(var y=0;y<cells.GetUpperBound(1);y++)
        {
            for (var x = 0; x<cells.GetUpperBound(0); x++)
            {
                action(x, y, cells[x, y]);
            }
        }
    }

    public static int ResolveDuration(this Cell cell, Battle battle) =>
        cell.Type switch
        {
            CellType.Land => battle.Settings.MovementDurations.MoveOverLandInMilliseconds,
            CellType.Forrest => battle.Settings.MovementDurations.MoveThroughForrestInMilliseconds,
            CellType.Mountain => throw new Exception("You can't move through mountains. You're not a dwarf from Lord of the Rings."),
            CellType.Swamp => throw new Exception("You can't move through a swamp unless you identify as a boat."),
            _ => throw new Exception("You cannot move to this cell")
        };
}