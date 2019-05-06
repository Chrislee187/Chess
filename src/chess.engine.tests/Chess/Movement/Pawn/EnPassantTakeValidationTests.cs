using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Chess.Movement;
using chess.engine.Chess.Movement.Validators;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Chess.Movement.Pawn
{
    [TestFixture]
    public class EnPassantTakeValidationTests
    {
        private 
            EasyBoardBuilder _board;
        private IBoardState<ChessPieceEntity> _boardState;
        private EnPassantTakeValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _board = new EasyBoardBuilder()
                .Board("   qk  r" +
                       "        " +
                       "Pp Pb PP" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K  R"
                );
            var game = new ChessGame(_board.ToGameSetup());
            _boardState = game.BoardState;
            _validator = new EnPassantTakeValidator();
        }

        [Test]
        public void Should_return_true_for_valid_take()
        {
            var promote = BoardMove.CreateTakeOnly(BoardLocation.At("A6"), BoardLocation.At("B7"));
            Assert.True(_validator.ValidateMove(promote, _boardState));
        }

        [Test]
        public void Should_return_false_when_no_piece_in_passing_location()
        {
            _boardState.Remove(BoardLocation.At("B6"));
            var promote = BoardMove.CreateTakeOnly(BoardLocation.At("A6"), BoardLocation.At("B7"));
            Assert.False(_validator.ValidateMove(promote, _boardState));
        }

        [TestCase("D6", "E7")]
        [TestCase("G6", "H7")]
        public void Should_return_false_when_wrong_piece_in_passing_location(string from, string to)
        {
            var promote = BoardMove.CreateTakeOnly(BoardLocation.At(from), BoardLocation.At(to));
            Assert.False(_validator.ValidateMove(promote, _boardState));
        }


    }
}