using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Extensions;
using chess.engine.Game;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class DestinationIsEmptyValidationTests
    {
        private DestinationIsEmptyValidator<ChessPieceEntity> _validator;
        private Mock<DestinationIsEmptyValidator<ChessPieceEntity>.IBoardStateWrapper> _wrapperMock;

        [SetUp]
        public void SetUp()
        {
            // TODO: Make all the validator tests use this mock approach to avoid creating a whole board.
            _validator = new DestinationIsEmptyValidator<ChessPieceEntity>();
            _wrapperMock = new Mock<DestinationIsEmptyValidator<ChessPieceEntity>.IBoardStateWrapper>();
        }

        [Test]
        public void Should_return_true_for_move_to_empty_space()
        {
            var empty = BoardMove.Create("E1".ToBoardLocation(), "E2".ToBoardLocation(), (int)ChessMoveTypes.CastleKingSide);

            _wrapperMock.Setup(m => m.GetToEntity(It.IsAny<BoardMove>()))
                .Returns((LocatedItem<ChessPieceEntity>) null);

            Assert.True(_validator.ValidateMove(empty, _wrapperMock.Object));
        }

        [Test]
        public void Should_return_false_for_move_to_non_empty_space()
        {
            var notEmpty = BoardMove.Create("A1".ToBoardLocation(), "A8".ToBoardLocation(), (int)DefaultActions.MoveOnly);

            _wrapperMock.Setup(m => m.GetToEntity(It.IsAny<BoardMove>())).Returns(
                ChessFactory.LocatedItem(notEmpty.To, ChessPieceName.Rook, Colours.White)
                );

            Assert.False(_validator.ValidateMove(notEmpty, _wrapperMock.Object));
        }
    }

}