using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Movement;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class DestinationIsEmptyOrContainsEnemyValidationTests
    {
        private EasyBoardBuilder _board;
        private BoardState _boardState;
        private DestinationIsEmptyOrContainsEnemyValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _board = new EasyBoardBuilder()
                .Board("r   k  r" +
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
            _validator = new DestinationIsEmptyOrContainsEnemyValidator();
        }

        [Test]
        public void Should_return_true_for_move_to_empty_space()
        {
            var empty = ChessMove.Create("A1", "A2", ChessMoveType.MoveOnly);
            Assert.True(_validator.ValidateMove(empty, _boardState));
        }

        [Test]
        public void Should_return_true_for_move_to_enemy_piece()
        {
            var empty = ChessMove.Create("A1", "A8", ChessMoveType.MoveOrTake);
            Assert.True(_validator.ValidateMove(empty, _boardState));
        }

        [Test]
        public void Should_return_false_for_move_to_own_pieece()
        {
            var notEmpty = ChessMove.Create("A1", "E1", ChessMoveType.MoveOnly);
            Assert.False(_validator.ValidateMove(notEmpty, _boardState));
        }
    }
}