using Godot;
using GColl = Godot.Collections;

//[GlobalClass, Tool]
public partial class Unit : Path2D
{
    [Export]
    public Grid Grid = ResourceLoader.Load("res://assets/maps/_testing/Grid.tres") as Grid;

    [Export]
    public int MoveRange = 6;

    [Export]
    public Texture Skin { set; get; }

    [Export]
    public Vector2 SkinOffset = Vector2.Zero;

    [Export]
    public float MoveSpeed = 600.0F;

    [Export]
    public Vector2 Cell = Vector2.Zero;

    public bool IsSelected = false;
    public bool _isWalking = false;
    private Sprite2D _sprite;
    private AnimationPlayer _animPlayer;
    private PathFollow2D _pathFollow;

    public override void _Ready()
    {
        SetProcess(false);

        _pathFollow = GetNode<PathFollow2D>("PathFollow2D");
        _sprite = _pathFollow.GetNode<Sprite2D>("Sprite");
        SetSkin(this.Skin as Texture2D);
        SetSkinOffset(this.SkinOffset);
        _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        Vector2 gridPosition = Grid.CalculateGridPosition(Position);
        SetCell(gridPosition);
        Position = Grid.CalculateMapPosition(Cell);

        if (!Engine.IsEditorHint()) {
            Curve = new Curve2D();
        }
    }

    public override void _Process(double delta)
    {
        _pathFollow.Progress += MoveSpeed * (float)delta;

        if (_pathFollow.ProgressRatio >= 1.0)
        {
            SetIsWalking(false);
            _pathFollow.Progress = 0.0F; //FIXME: Something wrong with curve internal?
            Position = Grid.CalculateMapPosition(Cell);
            Curve.ClearPoints();
            EmitSignal("WalkFinished");
        }
    }

    public void SetCell(Vector2 value)
    {
        Cell = Grid.GridClamp(value);
    }

    public void SetIsSelected(bool value)
    {
        IsSelected = value;

        if (IsSelected)
        {
            _animPlayer.Play("selected");
        }
        else
        {
            _animPlayer.Play("idle");
        }
    }

     public async void SetSkin(Texture2D value)
     {
        Skin = value;

        if (_sprite == null)
        {
            await ToSignal(this, "ready");
        }
        _sprite.Texture = value;
     }

     public async void SetSkinOffset(Vector2 value)
     {
        SkinOffset = value;
        if (_sprite == null)
        {
            await ToSignal(this, "ready");
        }
        _sprite.Position = value;
     }

     public void WalkAlong(GColl.Array<Vector2> path)
     {
        if (path.Count == 0)
        {
            GD.Print("[ERROR] Unit: Empty path provided to WalkAlong()");
            return;
        }

        Curve.AddPoint(Vector2.Zero);
        foreach (Vector2 point in path)
        {
            Curve.AddPoint(Grid.CalculateMapPosition(point) - Position);
        }

        SetCell(path[^1]);
        SetIsWalking(true);
     }

    private void SetIsWalking(bool value)
    {
        _isWalking = value;
        SetProcess(_isWalking);
    }

}