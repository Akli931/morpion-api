namespace MorpionApi.Models;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();


    public string?[] Board { get; set; } = new string?[9];

    public string CurrentPlayer { get; set; } = "X";

    public GameStatus Status { get; set; } = GameStatus.InProgress;

    public string? Winner { get; set; }
}

public enum GameStatus
{
    InProgress,
    Finished
}