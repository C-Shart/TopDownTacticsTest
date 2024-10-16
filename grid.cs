using Godot;

public partial class Grid : Resource
{
    [Export]
    public Vector2 Size = new(70, 70);
    //public Vector2 Size { get; set; };

    [Export]
    public Vector2 CellSize = new(70, 70);
    //public Vector2 CellSize { get; set; };

private Vector2 _halfCellSize;

    public Grid()
    {
        _halfCellSize = CellSize / 2;
    }

    // Grid coordinates to screen coordinates
    public Vector2 CalculateMapPosition(Vector2 gridPosition)
    {
        return gridPosition * CellSize + _halfCellSize;
    }

    // Screen coordinates to grid coordinates
    public Vector2 CalculateGridPosition(Vector2 mapPosition)
    {
        //return Math.Floor(mapPosition / CellSize);
        Vector2 converted = (mapPosition / CellSize).Floor();
        return new Vector2((int)converted.X, (int)converted.Y);
    }

    public bool IsWithinBounds(Vector2 cellCoordinates)
    {
        bool inside_x = cellCoordinates.X >= 0 && cellCoordinates.X < Size.X;
        bool inside_y = cellCoordinates.Y >= 0 && cellCoordinates.Y < Size.Y;
        return inside_x && inside_y;
    }

    public Vector2 GridClamp(Vector2 gridPosition)
    {
        return gridPosition.Clamp(
            Vector2.Zero,
            new(Size.X-1, Size.Y-1)
        );
    }

    public int AsIndex(Vector2 cell)
    {
        return (int)(cell.X + Size.X * cell.Y);
    }
}