using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export]
	private float Speed = 3.0f;

	[Export]
	private float TurningSidesSpeedPenalty = 0.5f;

	[Export]
	private float MaxSpeedSides = 500f;

	[Export]
	private float MaxSpeed = 1500.0f;

	[Export]
	private float InertiaForce = 10.0f;

    [Export]
	private float JumpVelocity = -400.0f;

    private float VerticalCurrentJumpPosition { get; set; } = 0;

    private bool IsJumping { get; set; } = false;

	private RichTextLabel StatusTextSign { get; set; }
	private RichTextLabel VelocityTextSign { get; set; }
	private RichTextLabel DirectionTextSign { get; set; }

	private int FrameCount { get; set; } = 0;
	private const int MaxFrameCount = 20;

    public override void _Ready()
    {
        StatusTextSign = GetNodeOrNull<RichTextLabel>("%PlayerStatusTextSign");
        VelocityTextSign = GetNodeOrNull<RichTextLabel>("%PlayerVelocityTextSign");
        DirectionTextSign = GetNodeOrNull<RichTextLabel>("%PlayerDirectionTextSign");
    }

	private void ModifyTextLabels(Vector2 innertialForceDirection)
	{
		if (FrameCount <= MaxFrameCount)
		{
			FrameCount++;
			return;
		}

		VelocityTextSign.Text = "Velocity:" + Velocity;
		if (innertialForceDirection != Vector2.Zero) {
			if (innertialForceDirection.X is -1)
				StatusTextSign.Text = "Status: Moving Left";
			else if (innertialForceDirection.X is 1)
				StatusTextSign.Text = "Status: Moving Right";
			if (innertialForceDirection.Y is -1)
				StatusTextSign.Text = "Status: Accellerating";
			else if (innertialForceDirection.Y is 1)
				StatusTextSign.Text = "Status: Descelerating";
		} else StatusTextSign.Text = "Status: Iddle";

		DirectionTextSign.Text = "Direction Vector:" + innertialForceDirection;

		FrameCount = 0;
	}

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

        Vector2 direction = Input.GetVector("left", "right", "descelerate", "accelerate");
		Vector2 innertialForceDirection = Vector2.Zero;
		innertialForceDirection.X = Math.Sign(velocity.X);
		innertialForceDirection.Y = Math.Sign(velocity.Y);


		ModifyTextLabels(innertialForceDirection);
		
		if (direction.X is not 0)
		{
			velocity.X += direction.X * Speed * TurningSidesSpeedPenalty;
			if (Math.Abs(velocity.X) > MaxSpeedSides)
				velocity.X = direction.X * MaxSpeedSides;
		} else {
			velocity.X -= innertialForceDirection.X * Math.Min(InertiaForce, Math.Abs(velocity.X));
		}
			

        if (direction.Y is not 0)
		{
			velocity.Y += direction.Y * Speed;
			if (Math.Abs(velocity.Y) > MaxSpeed)
				velocity.Y = direction.Y * MaxSpeed;
		} else {
			velocity.Y -= innertialForceDirection.Y * Math.Min(InertiaForce, Math.Abs(velocity.Y));
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
