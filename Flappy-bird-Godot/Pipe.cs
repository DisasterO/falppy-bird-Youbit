using Godot;
using System;

public partial class Pipe : Area2D
{
    public override void _Ready()
    {
    }

    public override void _Process(double tid)
    {
        // Skapar en hastighetsvektor som gör att röret rör sig åt vänster med en hastighet på 200 pixlar per sekund.
        var velocity = new Vector2(-200, 0);

        // Uppdaterar positionen på röret genom att flytta det baserat på hastigheten och tiden (tid).
        Position += velocity * (float)tid;
    }

    // Denna funktion anropas när röret har lämnat skärmen.
    public void PipeExitedScreen()
    {
        // Tar bort röret från spelet när det är utanför skärmen.
        QueueFree();
    }
}
