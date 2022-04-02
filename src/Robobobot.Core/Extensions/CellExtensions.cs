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
}