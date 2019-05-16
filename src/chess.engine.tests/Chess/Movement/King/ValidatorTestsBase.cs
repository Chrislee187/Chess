using chess.engine.Chess;
using chess.engine.Chess.Entities;

namespace chess.engine.tests.Chess.Movement.King
{
    public class ValidatorTestsBase
    {
        protected readonly ChessBoardEngineProvider ChessBoardEngineProvider = ChessFactory.ChessBoardEngineProvider();

        protected readonly ChessPieceEntityProvider ChessBoardEntityProvider = ChessFactory.ChessPieceEntityProvider();
        protected readonly IPlayerStateService PlayerStateService = ChessFactory.ChessGameStateService();
    }
}