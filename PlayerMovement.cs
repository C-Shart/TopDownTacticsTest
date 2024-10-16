using Godot;
using System;


public partial class PlayerMovement : CharacterBody2D
{
    [Export]
    private int speed = 50;
    private Vector2 currentVelocity;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        HandleInput();

        Velocity = currentVelocity;
        MoveAndSlide();

    }

    private void HandleInput()
    {
        currentVelocity = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        currentVelocity *= speed;
    }

}
