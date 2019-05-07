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
    public class DestinationIsEmptyValidationTests : ValidatorTestsBase
    {
        private EasyBoardBuilder _board;
        private IBoardState<ChessPieceEntity> _boardState;
        private DestinationIsEmptyValidator<ChessPieceEntity> _validator;

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
            var game = new ChessGame(
                new ChessRefreshAllPaths(MockLogger<ChessRefreshAllPaths>()), 
                    _board.ToGameSetup(), 
                    new ChessPathsValidator(new ChessPathValidator(validationFactory)));
            _boardState = game.BoardState;
            _validator = new DestinationIsEmptyValidator<ChessPieceEntity>();
        }

        [Test]
        public void Should_return_true_for_move_to_empty_space()
        {
            var empty = BoardMove.Create("E1", "E2", MoveType.CastleKingSide);
            Assert.True(_validator.ValidateMove(empty, _boardState));
        }

        [Test]
        public void Should_return_false_for_move_to_non_empty_space()
        {
            var notEmpty = BoardMove.Create("A1", "A8", MoveType.CastleQueenSide);
            Assert.False(_validator.ValidateMove(notEmpty, _boardState));
        }
    }

}