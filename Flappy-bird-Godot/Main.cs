using Godot;
using System;

public partial class Main : Node
{
    [Export]
    public PackedScene pipeScene { get; set; }
    
    // Håller reda på poängen i spelet.
    int score;

    // Reference to the Label node where the score will be displayed.
    private Label scoreLabel;
    // Denna funktion körs när scenen laddas in och spelet startar.
   public override void _Ready()
{
    // Update this path based on your actual node structure
    scoreLabel = GetNode<Label>("Lable"); 

    NewGame();
}

    // Startar ett nytt spel och återställer poängen. Flyttar fågeln till sin startposition.
     public void NewGame()
    {
        score = 0;  // Återställer poängen till 0.

        // Hämta fågeln och dess startposition från scenen.
        var player = GetNode<Bird>("Bird");
        var startPosition = GetNode<Marker2D>("BirdSpawnPosition");

        // Flyttar fågeln till sin startposition.
        player.Position = startPosition.Position;

        // Startar en timer som används för att regelbundet skapa nya rör.
        GetNode<Godot.Timer>("Timer").Start();
    }

    // Denna funktion körs varje gång ett nytt rör ska skapas.
    private void OnPipeSpawn()
    {
        // Skapar ett nytt rör baserat på den sparade rör-scenen (pipeScene).
        Pipe pipe = pipeScene.Instantiate<Pipe>();

        // Hämta positionen där röret ska skapas.
        var pipeSpawn = GetNode<Marker2D>("PipeSpawnPosition");
        var position = pipeSpawn.Position;

        // Slumpmässigt justera rörens höjd inom ett visst område för variation.
        position.Y += Mathf.Round(GD.RandRange(-12, 12)) * 9;
        pipe.Position = position;

        // Lägg till röret i scenen så det blir synligt och kan börja röra sig.
        AddChild(pipe);
    }

    // Denna funktion körs när fågeln dör.
    public void OnBirdDied()
    {
        GD.Print("bird died"); 

        var tree = GetTree();
        tree.Paused = true;  

        // Använd CallDeferred för att säkert ladda om scenen efter att fysikuppdateringen är klar.
        CallDeferred(nameof(DeferredReloadScene));
    }

    // Denna funktion laddar om spelet och återställer det till sitt ursprungsläge.
    private void DeferredReloadScene()
    {
        var tree = GetTree();
        tree.ReloadCurrentScene();  // Laddar om scenen så spelet börjar om från början.
        tree.Paused = false;  // Spelet återupptas.
    }

    // Denna funktion uppdaterar poängen när fågeln får poäng.
    public void OnAddScore(int scoreToAdd)
    {
        score += scoreToAdd;  // Lägger till poäng.
        GD.Print("score: " + score); 
        UpdateScoreLabel();
    }
    private void UpdateScoreLabel()
    {
        scoreLabel.Text = score.ToString();
    }
}
