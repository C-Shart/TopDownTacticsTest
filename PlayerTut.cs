using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public partial class PlayerTut : Area2D
{
    [Signal]
    public delegate void HitEventHandler();

    [Export]
    public int Speed { get; set; } = 400;

    public Vector2 ScreenSize;

    private string lastAnimDirection = "down";
    private Vector2 velocity = Vector2.Zero;
    private AnimatedSprite2D playerSprite;

    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }

    public override void _Ready()
    {
        playerSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        ScreenSize = GetViewportRect().Size;
    }

    public override void _Process(double delta)
    {

        HandleInput();
        UpdateAnimation(delta);

    }

    public void HandleInput()
    {
        var attacking = (playerSprite.Animation == "attack down" || playerSprite.Animation == "attack side" || playerSprite.Animation == "attack up") && playerSprite.IsPlaying();

        if (Input.IsActionJustPressed("attack"))
        {
            GD.Print($"ATTACK: Attack pressed");
            Attack();
        }
        else
        {
            velocity = Vector2.Zero;

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
        }

    }

    public void UpdateAnimation(double delta)
    {
        var attacking = (playerSprite.Animation == "attack down" || playerSprite.Animation == "attack side" || playerSprite.Animation == "attack up") && playerSprite.IsPlaying();

        if (!attacking)
        {
            MoveCharacter(delta);
        }
    }

    public void MoveCharacter(double delta)
    {
        if (velocity.Length() == 0)
            playerSprite.Play($"idle {lastAnimDirection}");
        else
        {
            string direction = "down";
            if (velocity.X != 0)
            {
                direction = "side";
                playerSprite.FlipH = velocity.X > 0;
            }
            else if (velocity.Y < 0)
            {
                direction = "up";
            }
            velocity = velocity.Normalized() * Speed;
            playerSprite.Play($"walk {direction}");
            lastAnimDirection = direction;
        }
        Position += velocity * (float)delta;
    }

    public void Attack()
    {
        GD.Print($"ATTACK:");

        var currentAnimation = playerSprite.Animation;
        GD.Print($"StartAnimation : { currentAnimation }");

        bool isSide = lastAnimDirection == "side";
        bool isDown = lastAnimDirection == "down";
        bool isUp = lastAnimDirection == "up";

        // Logic to attack or play animation.
        // Intended behavior: Stop movement momentarily, play attack animation
        // animatedSprite2D.Animation = "attack ";

        playerSprite.Stop();

        if (isSide)
        {
            playerSprite.Play("attack side");
        }
        else if (isUp)
        {
            playerSprite.Play("attack up");
        }
        else if (isDown)
        {
            playerSprite.Play("attack down");
        }
        else
        {
            GD.Print("ERROR: Attack: What did you do?");
        }

        GD.Print($"EndAnimation   : {playerSprite.Animation}");
        GD.Print($"FlipH          : {playerSprite.FlipH}");
    }

    private void OnBodyEntered(Node2D body)
    {
        Hide();
        EmitSignal(SignalName.Hit);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
}
