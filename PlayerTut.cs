using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class PlayerTut : Area2D
{
    [Signal]
    public delegate void HitEventHandler();

    [Export]
    public int Speed { get; set; } = 400;

    public Vector2 ScreenSize;

    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }

    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
    }

    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero;
        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // Control inputs
        if (Input.IsActionPressed("ui_right"))
        {
            velocity.X +=1;
        }
        if (Input.IsActionPressed("ui_left"))
        {
            velocity.X -=1;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            velocity.Y +=1;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            velocity.Y -=1;
        }
        if (Input.IsActionPressed("attack"))
        {
            // Logic to attack or play animation.
        }
        


        // Animation logic based on character movement
        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite2D.Play();
        }
        else
        {
            animatedSprite2D.Stop();
        }

        Position += velocity * (float)delta;

        // Supposed to clamp player within screen edges, but instead clamps them to the edge?
        /* Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
            y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
        ); */

        // Changes animation depending on movement direction
        if (velocity.X != 0)
        {
            animatedSprite2D.Animation = "walk side";
            animatedSprite2D.FlipV = false;
            animatedSprite2D.FlipH = velocity.X > 0;
        }
        else if (velocity.Y < 0)
        {
            animatedSprite2D.Animation = "walk up";
        }
        else if (velocity.Y > 0)
        {
            animatedSprite2D.Animation = "walk down";
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        Hide();
        EmitSignal(SignalName.Hit);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
}
