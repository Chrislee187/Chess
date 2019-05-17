using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using board.engine.tests.Actions;
using Moq;
using NUnit.Framework;

namespace board.engine.tests.Movement
{
    [TestFixture]
    public class DestinationIsEmptyValidationTests
    {
        private DestinationIsEmptyValidator<TestBoardEntity> _validator;
        private Mock<DestinationIsEmptyValidator<TestBoardEntity>.IBoardStateWrapper> _wrapperMock;

        [SetUp]
        public void SetUp()
        {
            _validator = new DestinationIsEmptyValidator<TestBoardEntity>();
            _wrapperMock = new Mock<DestinationIsEmptyValidator<TestBoardEntity>.IBoardStateWrapper>();
        }

        [Test]
        public void Should_return_true_for_move_to_empty_space()
        {
            var move = BoardMove.Create(BoardLocation.At(5,1), BoardLocation.At(5,2), (int)ChessMoveTypes.CastleKingSide);

            SetupNullToEntity(move);

            Assert.True(_validator.ValidateMove(move, _wrapperMock.Object));
        }

        [Test]
        public void Should_return_false_for_move_to_non_empty_space()
        {
            var move = BoardMove.Create(BoardLocation.At(1, 1), BoardLocation.At(1, 8), (int)DefaultActions.MoveOnly);

            SetupToEntity(move,1);

            Assert.False(_validator.ValidateMove(move, _wrapperMock.Object));
        }


        protected void SetupToEntity(BoardMove move, int owner)
        {
            var item = new LocatedItem<TestBoardEntity>(move.From, new TestBoardEntity(owner), new Paths());
            _wrapperMock.Setup(m => m.GetToEntity(It.IsAny<BoardMove>()))
                .Returns(item);
        }
        protected void SetupNullToEntity(BoardMove move)
        {
            _wrapperMock.Setup(m => m.GetToEntity(It.IsAny<BoardMove>()))
                .Returns((LocatedItem<TestBoardEntity>) null);
        }
    }

}