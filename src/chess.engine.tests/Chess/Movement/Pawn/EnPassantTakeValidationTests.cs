using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Chess.Movement.ChessPieces.Pawn;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
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
            var board = new EasyBoardBuilder()
                .Board("   qk  r" +
                       "        " +
                       "Pp Pb PP" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K  R"
                );
            IMoveValidationFactory<ChessPieceEntity> validationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(NullLogger<ChessGame>.Instance, ChessBoardEngineProvider, board.ToGameSetup());
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