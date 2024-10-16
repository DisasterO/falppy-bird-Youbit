using Godot;
using System;

public partial class Bird : CharacterBody2D
{
    [Signal]
    public delegate void AddScoreEventHandler();

    // Skapar en signal som skickas när fågeln dör och inkluderar poäng som ska läggas till.
    [Signal]
    public delegate void BirdDiedEventHandler(int scoreToAdd);

    // Variabel som bestämmer hur stark fågelns hopp är.
    [Export]
    public float hoppaStrenght = -500;

    // Hämtar gravitationsvärdet från spelets globala inställningar.
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
    }

    // Denna funktion körs för att uppdatera fysiken, som till exempel rörelse.
    public override void _PhysicsProcess(double tid)
    {
        // Hämtar nuvarande hastighet på fågeln.
        Vector2 myVelocity = Velocity;

        // Lägg till gravitation på fågelns hastighet så den faller nedåt över tid.
        myVelocity.Y += Gravity * (float)tid;

        // Om spelaren trycker på hopp knappen hoppar fågeln uppåt.
        if (Input.IsActionJustPressed("hoppa"))
        {
            myVelocity.Y = hoppaStrenght;  // Sätter fågelns hastighet uppåt.
        }

        // Uppdaterar fågelns hastighet med den nya hastigheten.
        Velocity = myVelocity;

        // Flyttar fågeln och gör så att den glider på ytor om den kolliderar.
        MoveAndSlide();
    }

    // Den här funktionen körs när fågeln kolliderar med något. 
    public void OnCollision(Rid areaRid, Node2D area, long areaShapeIndex, long localShapeIndex)
    {
        // Används för att se vad fågeln krockade med.
        GD.Print(areaShapeIndex);

        // Om fågeln träffar något med formindex skrivs det två och han inte dör.
        if (areaShapeIndex == 2)
        {
            // Skicka en signal att lägga till poäng, skickar med värdet 1.
            EmitSignal(SignalName.AddScore, 1);
        }
        else
        {
            // Om fågeln träffar något annat, skicka en signal att fågeln dog.
            EmitSignal(SignalName.BirdDied);
        }
    }
}
