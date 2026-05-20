using Microsoft.AspNetCore.Mvc;
using MorpionApi.Models;
using MorpionApi.Services;

namespace MorpionApi.Controllers;

[ApiController]
[Route("api/games")]
public class GameController(GameStore store) : ControllerBase
{
    // POST /api/games → Création d'une partie
    [HttpPost]
    public ActionResult<CreateGameResponse> CreateGame()
    {
        var game = store.Create();
        return CreatedAtAction(
            nameof(GetGame),
            new { id = game.Id },
            new CreateGameResponse(game.Id, "Partie créée ! Vous jouez X, le bot joue O.")
        );
    }

    // GET /api/games/{id} → Récupération de la partie en cours
    [HttpGet("{id:guid}")]
    public ActionResult<GameStateResponse> GetGame(Guid id)
    {
        var game = store.Get(id);
        if (game is null) return NotFound("Partie introuvable.");
        return Ok(ToResponse(game));
    }

    // POST /api/games/{id}/move → Jouer un coup
    [HttpPost("{id:guid}/move")]
    public ActionResult<GameStateResponse> PlayMove(Guid id, [FromBody] PlayMoveRequest request)
    {
        var game = store.Get(id);
        if (game is null) return NotFound("Partie introuvable.");

        if (game.Status == GameStatus.Finished)
            return BadRequest("La partie est déjà terminée.");

        if (request.Position < 0 || request.Position > 8)
            return BadRequest("Position invalide (0 à 8).");

        if (game.Board[request.Position] != null)
            return BadRequest("Cette case est déjà occupée.");

        // Coup du joueur (X)
        game.Board[request.Position] = "X";
        if (CheckEnd(game)) return Ok(ToResponse(game));

        // Coup du bot (O)
        int botMove = BotService.GetBotMove(game.Board);
        game.Board[botMove] = "O";
        CheckEnd(game);

        return Ok(ToResponse(game));
    }

    private static bool CheckEnd(Game game)
    {
        var winner = BotService.CheckWinner(game.Board);
        if (winner != null)
        {
            game.Status = GameStatus.Finished;
            game.Winner = winner;
            return true;
        }
        if (BotService.IsDraw(game.Board))
        {
            game.Status = GameStatus.Finished;
            return true;
        }
        return false;
    }

    private static GameStateResponse ToResponse(Game game) => new(
        game.Id,
        game.Board,
        game.CurrentPlayer,
        game.Status.ToString(),
        game.Winner
    );
}