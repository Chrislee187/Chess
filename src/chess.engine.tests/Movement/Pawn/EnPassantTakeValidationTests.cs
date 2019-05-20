using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using board.engine.tests.Movement;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement.Pawn;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Pawn
{
    [TestFixture]
    public class EnPassantTakeValidationTests : ValidationTestsBase
    {
        //TODO: Remove board builder and RoBoardStateMock
        private IBoardState<ChessPieceEntity> _boardState;
        private EnPassantTakeValidator _validator;
        [SetUp]
        public void SetUp()
        {
            var board = new ChessBoardBuilder()
                .Board("   qk  r" +
                       "        " +
                       "Pe Pb PP" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K  R"
                );
            _boardState = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.White).BoardState;
            _validator = new EnPassantTakeValidator();
        }

        [Test]
        public void Should_return_true_for_valid_take()
        {
            BoardLocation to = "B7".ToBoardLocation();
            var move = new BoardMove("A6".ToBoardLocation(), to, (int)DefaultActions.TakeOnly);
            Assert.True(_validator.ValidateMove(move, _boardState));
        }

        [Test]
        public void Should_return_false_when_no_piece_in_passing_location()
        {
            _boardState.Remove("B6".ToBoardLocation());
            BoardLocation to = "B7".ToBoardLocation();
            var move = new BoardMove("A6".ToBoardLocation(), to, (int) DefaultActions.TakeOnly);
            Assert.False(_validator.ValidateMove(move, _boardState));
        }

        [TestCase("D6", "E7")]
        [TestCase("G6", "H7")]
        public void Should_return_false_when_wrong_piece_in_passing_location(string from, string to)
        {
            BoardLocation to1 = to.ToBoardLocation();
            var move = new BoardMove(@from.ToBoardLocation(), to1, (int) DefaultActions.TakeOnly);
            Assert.False(_validator.ValidateMove(move, _boardState));
        }


    }
}