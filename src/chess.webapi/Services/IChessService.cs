using chess.engine.Game;

namespace chess.webapi.Services
{
    public interface IChessService
    {
        ChessWebApiResult GetNewBoard();
        ChessWebApiResult PlayMove(string board, string move);
        ChessWebApiResult GetMoves(string board);
        ChessWebApiResult GetMovesForPlayer(string board, Colours forPlayer);
        ChessWebApiResult GetMovesForLocation(string board, string location);
    }
}