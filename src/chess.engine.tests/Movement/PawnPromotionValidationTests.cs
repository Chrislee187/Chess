using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.SimpleValidators;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class PawnPromotionValidationTests
    {
        private EasyBoardBuilder _board;
        private BoardState _boardState;

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
            var game = new ChessGame(_board.ToGameSetup());
            _boardState = game.BoardState;
        }

        [Test]
        public void Should_return_true_for_valid_promotion()
        {
            var validator = new UpdatePieceValidator();

            var promote = BoardMove.CreatePawnPromotion(BoardLocation.At("A7"),BoardLocation.At("A8"), ChessPieceName.Queen);
            Assert.True(validator.ValidateMove(promote, _boardState));
        }
    }
}