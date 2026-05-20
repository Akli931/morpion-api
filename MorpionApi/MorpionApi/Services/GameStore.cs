using MorpionApi.Models;

namespace MorpionApi.Services;

public class GameStore
{
    private readonly Dictionary<Guid, Game> _games = new();

    public Game Create()
    {
        var game = new Game();
        _games[game.Id] = game;
        return game;
    }

    public Game? Get(Guid id) =>
        _games.TryGetValue(id, out var game) ? game : null;
}