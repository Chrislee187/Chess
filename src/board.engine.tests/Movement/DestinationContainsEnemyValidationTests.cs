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
    public class DestinationContainsEnemyValidationTests 
    {
        private Mock<DestinationContainsEnemyMoveValidator<TestBoardEntity>.IBoardStateWrapper> _wrapperMock;
        private DestinationContainsEnemyMoveValidator<TestBoardEntity> _validator;

        [SetUp]
        public void SetUp()
        {
            _wrapperMock = new Mock<DestinationContainsEnemyMoveValidator<TestBoardEntity>.IBoardStateWrapper>();
            _validator = new DestinationContainsEnemyMoveValidator<TestBoardEntity>();
       }

        [Test]
        public void Should_return_true_for_valid_take()
        {
            var move = BoardMove.Create(BoardLocation.At(1,1), BoardLocation.At(1, 8),
                (int) DefaultActions.MoveOrTake);


            SetupFromEntity(move, 1);
            SetupToEntity(move, 2);

            Assert.True(_validator.ValidateMove(move, _wrapperMock.Object));
        }

        [Test]
        public void Should_return_false_for_invalid_take()
        {
            var move = BoardMove.Create(BoardLocation.At(5, 8), BoardLocation.At(7, 8),
                (int) DefaultActions.MoveOrTake);

            SetupFromEntity(move, 1);
            SetupToEntity(move, 2);
            SetupNullToEntity();

            Assert.False(_validator.ValidateMove(move, _wrapperMock.Object));
        }

        private void SetupNullToEntity()
        {
            _wrapperMock.Setup(m => m.GetToEntity(It.IsAny<BoardMove>()))
                .Returns((LocatedItem<TestBoardEntity>)null);
        }
        protected void SetupToEntity(BoardMove move, int owner)
        {
            var item = new LocatedItem<TestBoardEntity>(move.To, new TestBoardEntity(owner), new Paths() );
            _wrapperMock.Setup(m => m.GetToEntity(It.IsAny<BoardMove>()))
                .Returns(item);
        }
        protected void SetupFromEntity(BoardMove move, int owner)
        {
            var item = new LocatedItem<TestBoardEntity>(move.To, new TestBoardEntity(owner), new Paths());

            _wrapperMock.Setup(m => m.GetFromEntity(It.IsAny<BoardMove>()))
                .Returns(item);
        }
    }
}