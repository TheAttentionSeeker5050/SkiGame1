using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : CharacterBody2D
{
    [Export]
	private float Speed = 1.5f;

	[Export]
	private float SlidingSidesXAxisSpeedPenalty = 0.5f;

	[Export]
	private float SlidingSidesYAxisSpeedBoost = 1.25f;

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
	private RichTextLabel ButtonPressTextSign { get; set; }
	private RichTextLabel CoordinatesTextSign { get; set; }

	private int FrameCount { get; set; } = 0;
	private const int MaxFrameCount = 20;

    public override void _Ready()
    {
        StatusTextSign = GetNodeOrNull<RichTextLabel>("%PlayerStatusTextSign");
        VelocityTextSign = GetNodeOrNull<RichTextLabel>("%PlayerVelocityTextSign");
        DirectionTextSign = GetNodeOrNull<RichTextLabel>("%PlayerDirectionTextSign");
        ButtonPressTextSign = GetNodeOrNull<RichTextLabel>("%PlayerButtonPressTextSign");
        CoordinatesTextSign = GetNodeOrNull<RichTextLabel>("%PlayerCoordinatesTextSign");
    }

	private void ModifyTextLabels(Vector2 innertialForceDirection, Vector2 inputDirection)
	{
		if (FrameCount <= MaxFrameCount)
		{
			FrameCount++;
			return;
		}

		VelocityTextSign.Text = "Velocity:" + Velocity.Floor();
		if (innertialForceDirection != Vector2.Zero) {
			if (innertialForceDirection.X is -1)
				StatusTextSign.Text = "Status: Moving Left";
			else if (innertialForceDirection.X is 1)
				StatusTextSign.Text = "Status: Moving Right";
			if (inputDirection.Y is 1)
				StatusTextSign.Text = "Status: Accellerating";
			else if (inputDirection.Y is -1)
				StatusTextSign.Text = "Status: Descelerating";
		} else StatusTextSign.Text = "Status: Iddle";

		if (Input.IsActionPressed("left"))
			ButtonPressTextSign.Text = "Button Pressed: Left";
		else if (Input.IsActionPressed("right"))
			ButtonPressTextSign.Text = "Button Pressed: Right";
		else if (Input.IsActionPressed("jump"))
			ButtonPressTextSign.Text = "Button Pressed: Jump";
		else if (Input.IsActionPressed("accelerate"))
			ButtonPressTextSign.Text = "Button Pressed: Accellerate";
		else if (Input.IsActionPressed("descelerate"))
			ButtonPressTextSign.Text = "Button Pressed: Descelerate";
		else if (Input.IsActionPressed("special_power"))
			ButtonPressTextSign.Text = "Button Pressed: Special Power";
		else if (Input.IsActionPressed("action"))
			ButtonPressTextSign.Text = "Button Pressed: Primary Action";
		else if (Input.IsActionPressed("hold_acceleration"))
			ButtonPressTextSign.Text = "Button Pressed: Hold Acceleration";
		else
			ButtonPressTextSign.Text = "Button Pressed: None";

		DirectionTextSign.Text = "Direction Vector:" + innertialForceDirection;

		CoordinatesTextSign.Text = "Player Coordinates" + this.Position.Floor();

		FrameCount = 0;
	}

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

        Vector2 inputDirection = Input.GetVector("left", "right", "descelerate", "accelerate");
		Vector2 innertialForceDirection = Vector2.Zero;
		innertialForceDirection.X = Math.Sign(velocity.X);
		innertialForceDirection.Y = Math.Sign(velocity.Y);


		ModifyTextLabels(innertialForceDirection, inputDirection);
		
		if (inputDirection.X is not 0)
		{
			velocity.X += inputDirection.X * Speed * SlidingSidesXAxisSpeedPenalty;
			if (Math.Abs(velocity.X) > MaxSpeedSides)
				velocity.X = inputDirection.X * MaxSpeedSides;
		} else {
			velocity.X -= innertialForceDirection.X * Math.Min(InertiaForce, Math.Abs(velocity.X));
		}
			

        if (inputDirection.Y is not 0)
		{
			
			if (inputDirection.Y > 0 && inputDirection.X is not 0)
			{
				velocity.Y += inputDirection.Y * Speed * SlidingSidesYAxisSpeedBoost;
				StatusTextSign.Text = "Accellerating and Sliding Boost";
			} else {
				velocity.Y += inputDirection.Y * Speed;
			}

			if (Math.Abs(velocity.Y) > MaxSpeed)
				velocity.Y = inputDirection.Y * MaxSpeed;

		} else {
			velocity.Y -= innertialForceDirection.Y * InertiaForce;
		}

		if (velocity.Y <= 0)
			velocity.Y = 0;

		Velocity = velocity;
		MoveAndSlide();
	}

	// TODO: Add a logic to jump

	// TODO: Add a logic to dodge

	// TODO: Add logic to hold accelleration with special button

	// TODO: Add logic to action (like hit the other player with ski cane or throw an object picked from the floor) and special power (like that kart game made by a company that will sue it if I even mention it) 
}
