using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Movement;
using chess.engine.Movement.SimpleValidators;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class DestinationNotUnderAttackValidationTests
    {
        private EasyBoardBuilder _board;
        private BoardState _boardState;
        private DestinationNotUnderAttackValidator _validator;

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
            _validator = new DestinationNotUnderAttackValidator();
        }

        [Test]
        public void Should_return_false_for_square_under_enemy_attack()
        {

            var containsEnemy = ChessMove.Create("E1", "D1", ChessMoveType.MoveOrTake);
            Assert.False(_validator.ValidateMove(containsEnemy, _boardState));

        }
        [Test]
        public void Should_return_true_for_square_under_friendly_attack()
        {
            var noEnemy = ChessMove.Create("E1", "F1", ChessMoveType.MoveOrTake);
            Assert.True(_validator.ValidateMove(noEnemy, _boardState));

        }
        [Test]
        public void Should_return_true_for_square_under_no_attack()
        {
            var noEnemy = ChessMove.Create("E1", "E2", ChessMoveType.MoveOrTake);
            Assert.True(_validator.ValidateMove(noEnemy, _boardState));

        }
    }
}