using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.tests.Chess.Movement.King;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class ChessPathsValidatorTests : ValidatorTestsBase
    {

        [Test]
        public void Should_find_move_that_leaves_king_in_check()
        {
            var board = new EasyBoardBuilder()
                .Board("    k   " +
                       "        " +
                       "        " +
                       "    p   " +
                       "   PQ   " +
                       "        " +
                       "        " +
                       "    K   "
                );
            IMoveValidationFactory<ChessPieceEntity> validationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(NullLogger<ChessGame>.Instance, ChessBoardEngineProvider, board.ToGameSetup());

            var blockedPieceLocation = BoardLocation.At("E5");

            var blockedPiece = game.BoardState.GetItem(blockedPieceLocation);

            Assert.False(blockedPiece.Paths.ContainsMoveTo(BoardLocation.At("D4")),
                $"Pawn at E5 should NOT be able to move D4");
        }

    }
}