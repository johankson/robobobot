namespace Robobobot.Core;

/// <summary>
/// Represents one cell or square of a map.
/// </summary>
public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    
    /// <summary>
    /// The type of terrain in the cell.
    /// </summary>
    public CellType Type { get; set; }

    /// <summary>
    /// Gets the cell type as its character representation.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public char GetCharType() => Type switch
    {
        CellType.Land => '.',
        CellType.Forrest => 'F',
        CellType.Mountain => 'M',
        CellType.Swamp => 'S',
        _ => throw new ArgumentOutOfRangeException()
    };
    
    /// <summary>
    /// Determines if the cell is transparent.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public bool IsSeeThrough() => Type switch
    {
        CellType.Land => true,
        CellType.Forrest => false,
        CellType.Mountain => false,
        CellType.Swamp => true,
        _ => throw new ArgumentOutOfRangeException()
    };
    
    /// <summary>
    /// Determines if the cell can be moved over.
    /// </summary>
    /// <returns></returns>
    public bool IsMovableTo() => Type is CellType.Forrest or CellType.Land;
}