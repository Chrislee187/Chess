using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.Validators;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class DestinationContainsEnemyValidationTests
    {
        private EasyBoardBuilder _board;
        private IBoardState<ChessPieceEntity> _boardState;

        [SetUp]
        public void SetUp()
        {
            _board = new EasyBoardBuilder()
                .Board("r  qk  r" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "R   K  R"
                );
            var game = new ChessGame(_board.ToGameSetup());
            _boardState = game.BoardState;
        }

        [Test]
        public void Should_return_true_for_valid_take()
        {
            var validator = new DestinationContainsEnemyMoveValidator<ChessPieceEntity>();

            var containsEnemy = BoardMove.Create("A1", "A8", MoveType.MoveOrTake);
            Assert.True(validator.ValidateMove(containsEnemy, _boardState));

        }
        [Test]
        public void Should_return_false_for_invalid_take()
        {
            var validator = new DestinationContainsEnemyMoveValidator<ChessPieceEntity>();

            var noEnemy = BoardMove.Create("E8", "G8", MoveType.MoveOrTake);
            Assert.False(validator.ValidateMove(noEnemy, _boardState));

        }
    }
    [TestFixture]
    public class ChessPathsValidatorTests
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
            var game = new ChessGame(board.ToGameSetup());

            var blockedPieceLocation = BoardLocation.At("E5");

            var blockedPiece = game.BoardState.GetItem(blockedPieceLocation);

            Assert.False(blockedPiece.Paths.ContainsMoveTo(BoardLocation.At("D4")),
                $"Pawn at E5 should NOT be able to move D4");
        }

    }
}