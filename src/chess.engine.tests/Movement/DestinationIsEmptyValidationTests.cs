using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Extensions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class DestinationIsEmptyValidationTests
    {

        private IBoardState<ChessPieceEntity> _boardState;
        private DestinationIsEmptyValidator<ChessPieceEntity> _validator;

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
            _validator = new DestinationIsEmptyValidator<ChessPieceEntity>();
        }

        [Test]
        public void Should_return_true_for_move_to_empty_space()
        {
            var empty = BoardMove.Create("E1".ToBoardLocation(), "E2".ToBoardLocation(), (int)ChessMoveTypes.CastleKingSide);
            Assert.True(_validator.ValidateMove(empty, _boardState));
        }

        [Test]
        public void Should_return_false_for_move_to_non_empty_space()
        {
            var notEmpty = BoardMove.Create("A1".ToBoardLocation(), "A8".ToBoardLocation(), (int)ChessMoveTypes.CastleQueenSide);
            Assert.False(_validator.ValidateMove(notEmpty, _boardState));
        }
    }

}