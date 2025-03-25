using System;
using System.Linq;
using Godot;

public partial class TestGreenScreen : Node2D
{

    // Declare the unique scene objects
    private CharacterBody2D Player {get; set;}
    private Node2D TileMapContainer {get; set;}
    private Node2D BackgroundContainer {get; set;}
    
    // We dont want to recalculate every frame on whether we should rendernew level parts or not so we will have a counter of number of frames to make this calc
    private const int NumOfFramesToCalcGenerateScenery = 15;
    private int FramesCounterToCalcGenerateScenery = 15;

    // Add to TestGreenScreen class properties
    private const float GenerationThreshold = 4000f; // Pixels from edge to trigger generation
    private Rect2 currentCanvasItemsBounds;

    private int BackgroundTop;
    private int BackgroundBottom;

    private const int rectHeight = 1000; // Match your ColorRect height
    private const int rectWidth = 10000; // Match your ColorRect height

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        try
        {
            // First we will get those unique nodes in our scene
            Player = GetNode<CharacterBody2D>("Player");
            TileMapContainer = GetNode<Node2D>("TileMapContainer");
            BackgroundContainer = GetNode<Node2D>("BackgroundContainer");

        }
        catch (Exception ex)
        {
            GD.PrintErr("Warning: some of the unique nodes could not be retrieved", ex.Message);
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        FramesCounterToCalcGenerateScenery++;
        if (FramesCounterToCalcGenerateScenery >= NumOfFramesToCalcGenerateScenery)
        {
            UpdateCanvasBackgroundBounds();
            CheckPlayerPosition();
            // Add populate canvas rectangles and tiles functions here
            FramesCounterToCalcGenerateScenery = 0;
        }
	}

    private void UpdateCanvasBackgroundBounds() 
    {
        var initialRectangle = BackgroundContainer.GetChild<ColorRect>(0);
        // set initial width and height and position
        initialRectangle.SetPosition(Vector2.Zero);
        
        Vector2 newSize = Vector2.Zero; 
        newSize.X = rectWidth;
        newSize.Y = rectHeight;
        if (newSize != Vector2.Zero)
            initialRectangle.SetSize(newSize);

        var rects = BackgroundContainer.GetChildren().Cast<ColorRect>();
        currentCanvasItemsBounds = rects.Any() 
            ? new Rect2(0, rects.Min(r => r.Position.Y), 
                    GetViewportRect().Size.X, 
                    rects.Max(r => r.Position.Y + rectHeight) - rects.Min(r => r.Position.Y))
            : new Rect2();
    }

    private void CheckPlayerPosition()
    {
        if(Player == null) return;

        var playerY = Player.Position.Y;
        var bufferZone = currentCanvasItemsBounds.Size.Y * 0.2f; // 20% of total height

        // Detect approach to top/bottom with buffer zone
        if(playerY < currentCanvasItemsBounds.Position.Y + GenerationThreshold || playerY > currentCanvasItemsBounds.End.Y - GenerationThreshold)
        {
            GenerateNewBackgroundCanvasRectangles();
        }
    }
    
    public void GenerateNewBackgroundCanvasRectangles()
    {
        // -------------------------------------------------------------
        // Generate new Color Rect and add it to the BackgroundContainer
        // -------------------------------------------------------------

        // Get existing rectangles sorted by Y position
        var rects = BackgroundContainer.GetChildren()
                .Cast<ColorRect>()
                .OrderBy(r => r.Position.Y)
                .ToList();
        
        // Calculate needed expansion direction
        bool needsTopRectangleRemoval = Player.Position.Y > currentCanvasItemsBounds.Position.Y + GenerationThreshold;
        bool needsBottomExpansion = Player.Position.Y > currentCanvasItemsBounds.End.Y - GenerationThreshold;

        // Deleting old background rectangles as the player advances
        if (needsTopRectangleRemoval) {
            RemoveFirstBackgroundRect();
        }
        
        // Adding new background rectangles as the player advances
        if (needsBottomExpansion)
        {
            float newY = rects.Any() ? rects.Last().Position.Y + rectHeight : 0;
            CreateBackgroundRect(newY);
        }
        
        UpdateCanvasBackgroundBounds();
    }

    private void CreateBackgroundRect(float yPosition)
    {
        var newRect = new ColorRect
        {
            Size = new Vector2(rectWidth, rectHeight),
            Position = new Vector2(0, yPosition),
            Color = Colors.White
        };
        
        BackgroundContainer.AddChild(newRect);
    }

    private void RemoveFirstBackgroundRect()
    {
        BackgroundContainer.GetChild(0).QueueFree();
    }

    // TODO: Add a logic to programatically add up new tiles as the player advances into the track
    public void GenerateNewTiles()
    {
        var playerPosition = Player.Position;
    }

    // TODO: Add a logic to programmatically delete bg rectangles and tiles from tilemap layer as the players advance: Keep in mind that players are instances nodes that have position properties, so it is easy, the player most behind's position tells what to delete from the start of the track. No need to go back as that is not possible in this game.

    // TODO: Add a logic to make a end part of the track, as it is being created programmatically we need to generate a finish line, or whatever the game mode later on consists of.

    // TODO: Add a logic to deal with obstacles (as tiles), ramps and players interacting with the obstacles to make the other players fall into these, example, player while skying can grab a rock and throw it to the player in front of him. the other player can make a dodge movement to evade it. Or if player sees a trash can or branch in the road, they can spread it so the other player behind will have it harder to evade this obstacle. Remember these are tile layers so they have to be dealt with as states in some way. Will have to check out this

    // TODO: Pass the support methods if possible to auxiliary classes to be called as static methods, or even if these depend on specific entities make classes and namespaces to these

    // TODO: Make this a parent class where levels can inherit and override from
}
