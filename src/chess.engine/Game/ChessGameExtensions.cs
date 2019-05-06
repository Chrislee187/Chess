using chess.engine.Board;
using chess.engine.Chess;

namespace chess.engine.Game
{
    public static class ChessGameExtensions
    {
        public static string ToText(this ChessGame game)
        {
            return new EasyBoardBuilder().FromChessGame(game).ToString();
        }
    }
}