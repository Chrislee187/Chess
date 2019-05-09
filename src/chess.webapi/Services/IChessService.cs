using chess.engine.Game;

namespace chess.webapi.Services
{
    public interface IChessService
    {
        ChessGameResult GetNewBoard();
        ChessGameResult PlayMove(string board, string move);
        ChessGameResult GetMoves(string board);
        ChessGameResult GetMovesForPlayer(string board, Colours forPlayer);
        ChessGameResult GetMovesForLocation(string board, string location);
    }
}