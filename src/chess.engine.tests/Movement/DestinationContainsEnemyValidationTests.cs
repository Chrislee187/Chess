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
    public class DestinationContainsEnemyValidationTests :ValidatorTestsBase
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
            IMoveValidationFactory<ChessPieceEntity> validationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(new ChessRefreshAllPaths(MockLogger<ChessRefreshAllPaths>()), _board.ToGameSetup(), new ChessPathsValidator(new ChessPathValidator(validationFactory)));
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
}