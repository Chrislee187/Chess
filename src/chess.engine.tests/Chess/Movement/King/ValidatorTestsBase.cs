using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Movement;
using Microsoft.Extensions.Logging.Abstractions;

namespace chess.engine.tests.Chess.Movement.King
{
    public class ValidatorTestsBase
    {
        protected readonly ChessBoardEngineProvider ChessBoardEngineProvider = new ChessBoardEngineProvider(
            NullLogger<BoardEngine<ChessPieceEntity>>.Instance,
            new ChessRefreshAllPaths(
                NullLogger<ChessRefreshAllPaths>.Instance, 
                new ChessGameState(NullLogger<ChessGameState>.Instance)),
            new ChessPathsValidator(
                NullLogger<ChessPathValidator>.Instance, 
                new ChessPathValidator(
                    NullLogger<ChessPathValidator>.Instance, 
                    new MoveValidationFactory<ChessPieceEntity>()
                    )));
    }
}