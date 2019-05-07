using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Movement;
using chess.engine.Movement.Validators;
using chess.engine.tests.Chess.Movement.King;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class DestinationIsEmptyOrContainsEnemyValidationTests :ValidatorTestsBase
    {
        private EasyBoardBuilder _board;
        private IBoardState<ChessPieceEntity> _boardState;
        private DestinationIsEmptyOrContainsEnemyValidator<ChessPieceEntity> _validator;

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
            IMoveValidationFactory<ChessPieceEntity> validationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(new ChessRefreshAllPaths(MockLogger<ChessRefreshAllPaths>()), _board.ToGameSetup(), new ChessPathsValidator(new ChessPathValidator(validationFactory)));
            _boardState = game.BoardState;
            _validator = new DestinationIsEmptyOrContainsEnemyValidator<ChessPieceEntity>();
        }

        [Test]
        public void Should_return_true_for_move_to_empty_space()
        {
            var empty = BoardMove.Create("A1", "A2", MoveType.MoveOnly);
            Assert.True(_validator.ValidateMove(empty, _boardState));
        }

        [Test]
        public void Should_return_true_for_move_to_enemy_piece()
        {
            var empty = BoardMove.Create("A1", "A8", MoveType.MoveOrTake);
            Assert.True(_validator.ValidateMove(empty, _boardState));
        }

        [Test]
        public void Should_return_false_for_move_to_own_piece()
        {
            var notEmpty = BoardMove.Create("A1", "E1", MoveType.MoveOnly);
            Assert.False(_validator.ValidateMove(notEmpty, _boardState));
        }
    }
}