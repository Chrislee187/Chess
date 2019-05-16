using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Chess.Entities;
using chess.engine.Extensions;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class DestinationIsEmptyOrContainsEnemyValidationTests
    {
        private IBoardState<ChessPieceEntity> _boardState;
        private DestinationIsEmptyOrContainsEnemyValidator<ChessPieceEntity> _validator;

        [SetUp]
        public void SetUp()
        {
            var board = new ChessBoardBuilder()
                .Board("r   k  r" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "R   K  R"
                );
            var game = ChessFactory.CustomChessGame(board.ToGameSetup());
            _boardState = game.BoardState;
            _validator = new DestinationIsEmptyOrContainsEnemyValidator<ChessPieceEntity>();
        }

        [Test]
        public void Should_return_true_for_move_to_empty_space()
        {
            var empty = BoardMove.Create("A1".ToBoardLocation(), "A2".ToBoardLocation(), (int) DefaultActions.MoveOnly);
            Assert.True(_validator.ValidateMove(empty, _boardState));
        }

        [Test]
        public void Should_return_true_for_move_to_enemy_piece()
        {
            var empty = BoardMove.Create("A1".ToBoardLocation(), "A8".ToBoardLocation(), (int) DefaultActions.MoveOrTake);
            Assert.True(_validator.ValidateMove(empty, _boardState));
        }

        [Test]
        public void Should_return_false_for_move_to_own_piece()
        {
            var notEmpty = BoardMove.Create("A1".ToBoardLocation(), "E1".ToBoardLocation(), (int) DefaultActions.MoveOnly);
            Assert.False(_validator.ValidateMove(notEmpty, _boardState));
        }
    }
}