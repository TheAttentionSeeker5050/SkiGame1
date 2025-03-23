using System;
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

	private RichTextLabel BackgroundBoundsTextSign { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        try
        {
            // First we will get those unique nodes in our scene
            Player = GetNode<CharacterBody2D>("Player");
            TileMapContainer = GetNode<Node2D>("TileMapContainer");
            BackgroundContainer = GetNode<Node2D>("BackgroundContainer");
            GD.Print("Unique nodes loaded successfully");

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
            // Add populate canvas rectangles and tiles functions here
            FramesCounterToCalcGenerateScenery = 0;
        }
	}

    // TODO: Add a logic to add new white background rectangles when the player is about to reach the bounds of the world
    public void GenerateNewBackgroundRectangles()
    {
        GD.Print("Generating new background rectangles");

        var playerPosition = Player.Position;
        

        // We need to have a logic that gets all the items in the rectangle set, gets the lower and upper bounds as the max and min positions of the items, calculate the edges and sees if there is a need for a list, or work with signals.
        // Perhaps we could store these edges into int variables and just make a rectangle as it is only snow and there is no need for somehting more refined, also the track is gonna bound the player anyways so it can only extend vertically
    }


    // TODO: Add a logic to programatically add up new tiles as the player advances into the track
    public void GenerateNewTiles()
    {
        GD.Print("Propping tilesets");
        var playerPosition = Player.Position;
    }

    // TODO: Add a logic to programmatically delete bg rectangles and tiles from tilemap layer as the players advance: Keep in mind that players are instances nodes that have position properties, so it is easy, the player most behind's position tells what to delete from the start of the track. No need to go back as that is not possible in this game.

    // TODO: Add a logic to make a end part of the track, as it is being created programmatically we need to generate a finish line, or whatever the game mode later on consists of.

    // TODO: Add a logic to deal with obstacles (as tiles), ramps and players interacting with the obstacles to make the other players fall into these, example, player while skying can grab a rock and throw it to the player in front of him. the other player can make a dodge movement to evade it. Or if player sees a trash can or branch in the road, they can spread it so the other player behind will have it harder to evade this obstacle. Remember these are tile layers so they have to be dealt with as states in some way. Will have to check out this

    // TODO: Pass the support methods if possible to auxiliary classes to be called as static methods, or even if these depend on specific entities make classes and namespaces to these

    // TODO: Make this a parent class where levels can inherit and override from
}
