using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Movement;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace chess.engine.Board
{
    public class HelperFactory
    {
        public static ChessGame NewChessGameNoLoggers =>
            new ChessGame(NullLogger<ChessGame>.Instance, ChessBoardEngineProviderNoLoggers);

        public static ChessBoardEngineProvider ChessBoardEngineProviderNoLoggers => new ChessBoardEngineProvider(
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