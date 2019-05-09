using chess.engine.Chess;
using chess.engine.Chess.Entities;

namespace chess.engine.tests.Chess.Movement.King
{
    public class ValidatorTestsBase
    {
        protected readonly ChessBoardEngineProvider ChessBoardEngineProvider = ChessFactory.ChessBoardEngineProvider();

        protected readonly ChessPieceEntityFactory ChessBoardEntityFactory = ChessFactory.ChessPieceEntityFactory();
        protected readonly IChessGameStateService ChessGameStateService = ChessFactory.ChessGameStateService();
    }
}