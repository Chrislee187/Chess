using chess.engine.Chess;
using chess.engine.Extensions;
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
            var board = new ChessBoardBuilder()
                .Board("    k   " +
                       "        " +
                       "        " +
                       "    p   " +
                       "   PQ   " +
                       "        " +
                       "        " +
                       "    K   "
                );
            var game = new ChessGame(NullLogger<ChessGame>.Instance, ChessBoardEngineProvider, ChessBoardEntityProvider, PlayerStateService, board.ToGameSetup());

            var blockedPieceLocation = "E5".ToBoardLocation();

            var blockedPiece = game.BoardState.GetItem(blockedPieceLocation);

            Assert.False(blockedPiece.Paths.ContainsMoveTo("D4".ToBoardLocation()),
                $"Pawn at E5 should NOT be able to move D4");
        }

    }
}