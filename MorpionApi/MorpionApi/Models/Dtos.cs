namespace MorpionApi.Models;

public record CreateGameResponse(Guid GameId, string Message);

public record PlayMoveRequest(int Position); // 0 à 8

public record GameStateResponse(
    Guid GameId,
    string?[] Board,
    string CurrentPlayer,
    string Status,
    string? Winner
);