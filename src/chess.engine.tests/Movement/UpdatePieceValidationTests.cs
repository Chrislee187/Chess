using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.Validators;
using chess.engine.tests.Chess.Movement.King;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class UpdatePieceValidationTests : ValidatorTestsBase
    {
        private EasyBoardBuilder _board;
        private IBoardState<ChessPieceEntity> _boardState;

        [SetUp]
        public void SetUp()
        {
            _board = new EasyBoardBuilder()
                .Board("   qk  r" +
                       "P       " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K  R"
                );
            IMoveValidationFactory<ChessPieceEntity> validationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(new ChessRefreshAllPaths(MockLogger<ChessRefreshAllPaths>()), _board.ToGameSetup(), new ChessPathsValidator(new ChessPathValidator(validationFactory)));
            _boardState = game.BoardState;
        }

        [Test]
        public void Should_return_true_for_valid_promotion()
        {
            var validator = new UpdatePieceValidator<ChessPieceEntity>();

            var promote = BoardMove.CreateUpdatePiece(BoardLocation.At("A7"),BoardLocation.At("A8"), ChessPieceName.Queen);
            Assert.True(validator.ValidateMove(promote, _boardState));
        }
    }
}