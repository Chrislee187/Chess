using board.engine;
using board.engine.Movement;

namespace chess.engine.Extensions
{
    public static class BoardLocationExtensions
    {
        public static string ToChessCoord(this BoardLocation loc)
        {
            return $"{(char)('A' + loc.X - 1)}{loc.Y}";
        }
    }

    public static class BoardMoveExtensions
    {
        public static string ToChessCoords(this BoardMove move)
        {
            return $"{move.From.ToChessCoord()}{move.To.ToChessCoord()}";
        }
    }
}