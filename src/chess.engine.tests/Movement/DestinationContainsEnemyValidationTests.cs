using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Extensions;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class DestinationContainsEnemyValidationTests
    {
        private ChessBoardBuilder _board;
        private IBoardState<ChessPieceEntity> _boardState;

        [SetUp]
        public void SetUp()
        {
            _board = new ChessBoardBuilder()
                .Board("r  qk  r" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "R   K  R"
                );
            var game = ChessFactory.NewChessGame();
            _boardState = game.BoardState;
        }

        [Test]
        public void Should_return_true_for_valid_take()
        {
            var validator = new DestinationContainsEnemyMoveValidator<ChessPieceEntity>();

            var containsEnemy = BoardMove.Create("A1".ToBoardLocation(), "A8".ToBoardLocation(), (int) DefaultActions.MoveOrTake);
            Assert.True(validator.ValidateMove(containsEnemy, _boardState));

        }
        [Test]
        public void Should_return_false_for_invalid_take()
        {
            var validator = new DestinationContainsEnemyMoveValidator<ChessPieceEntity>();

            var noEnemy = BoardMove.Create("E8".ToBoardLocation(), "G8".ToBoardLocation(), (int) DefaultActions.MoveOrTake);
            Assert.False(validator.ValidateMove(noEnemy, _boardState));

        }
    }
}