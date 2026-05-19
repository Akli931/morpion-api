namespace MorpionApi.Services;

public static class BotService
{
    private static readonly int[][] WinningLines =
    [
        [0, 1, 2], [3, 4, 5], [6, 7, 8], // lignes
        [0, 3, 6], [1, 4, 7], [2, 5, 8], // colonnes
        [0, 4, 8], [2, 4, 6]              // diagonales
    ];

    public static int GetBotMove(string?[] board)
    {
        // 1. Gagner si possible
        int? win = FindBestMove(board, "O");
        if (win.HasValue) return win.Value;

        // 2. Bloquer le joueur
        int? block = FindBestMove(board, "X");
        if (block.HasValue) return block.Value;

        // 3. Prendre le centre
        if (board[4] == null) return 4;

        // 4. Jouer aléatoirement
        var free = board
            .Select((v, i) => (v, i))
            .Where(x => x.v == null)
            .Select(x => x.i)
            .ToList();

        return free[Random.Shared.Next(free.Count)];
    }

    private static int? FindBestMove(string?[] board, string player)
    {
        foreach (var line in WinningLines)
        {
            var vals = line.Select(i => board[i]).ToArray();
            if (vals.Count(v => v == player) == 2 && vals.Any(v => v == null))
                return line[Array.IndexOf(vals, null)];
        }
        return null;
    }

    public static string? CheckWinner(string?[] board)
    {
        foreach (var line in WinningLines)
        {
            var first = board[line[0]];
            if (first != null && line.All(i => board[i] == first))
                return first;
        }
        return null;
    }

    public static bool IsDraw(string?[] board) => board.All(c => c != null);
}