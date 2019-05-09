using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Movement;
using chess.engine.Chess.Movement.ChessPieces.Pawn;
using chess.engine.Extensions;
using chess.engine.tests.Builders;
using chess.engine.tests.Chess.Movement.King;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace chess.engine.tests.Chess.Movement.Pawn
{
    [TestFixture]
    public class EnPassantTakeValidationTests : ValidatorTestsBase
    {
        private IBoardState<ChessPieceEntity> _boardState;
        private EnPassantTakeValidator _validator;

        [SetUp]
        public void SetUp()
        {
            var board = new ChessBoardBuilder()
                .Board("   qk  r" +
                       "        " +
                       "Pp Pb PP" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K  R"
                );
            _boardState = new ChessGameBuilder().BuildGame(board.ToGameSetup()).BoardState;
            _validator = new EnPassantTakeValidator();
        }

        [Test]
        public void Should_return_true_for_valid_take()
        {
            BoardLocation to = "B7".ToBoardLocation();
            var promote = new BoardMove("A6".ToBoardLocation(), to, (int)DefaultActions.TakeOnly);
            Assert.True(_validator.ValidateMove(promote, _boardState));
        }

        [Test]
        public void Should_return_false_when_no_piece_in_passing_location()
        {
            _boardState.Remove("B6".ToBoardLocation());
            BoardLocation to = "B7".ToBoardLocation();
            var promote = new BoardMove("A6".ToBoardLocation(), to, (int) DefaultActions.TakeOnly);
            Assert.False(_validator.ValidateMove(promote, _boardState));
        }

        [TestCase("D6", "E7")]
        [TestCase("G6", "H7")]
        public void Should_return_false_when_wrong_piece_in_passing_location(string from, string to)
        {
            BoardLocation to1 = to.ToBoardLocation();
            var promote = new BoardMove(@from.ToBoardLocation(), to1, (int) DefaultActions.TakeOnly);
            Assert.False(_validator.ValidateMove(promote, _boardState));
        }


    }
}