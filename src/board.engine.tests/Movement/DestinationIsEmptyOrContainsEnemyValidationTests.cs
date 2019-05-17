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
    public class DestinationIsEmptyOrContainsEnemyValidationTests
    {
        private DestinationIsEmptyOrContainsEnemyValidator<TestBoardEntity> _validator;
        private Mock<DestinationIsEmptyOrContainsEnemyValidator<TestBoardEntity>.IBoardStateWrapper> _wrapperMock;
        private Mock<DestinationIsEmptyValidator<TestBoardEntity>.IBoardStateWrapper> _isEmptyWrapperMock;
        private Mock<DestinationContainsEnemyMoveValidator<TestBoardEntity>.IBoardStateWrapper> _containsEnemyWrapperMock;

        [SetUp]
        public void SetUp()
        {

            _validator = new DestinationIsEmptyOrContainsEnemyValidator<TestBoardEntity>();
            _wrapperMock = new Mock<DestinationIsEmptyOrContainsEnemyValidator<TestBoardEntity>.IBoardStateWrapper>();

            _isEmptyWrapperMock = new Mock<DestinationIsEmptyValidator<TestBoardEntity>.IBoardStateWrapper>();
            _wrapperMock.Setup(m => m.GetDestinationIsEmptyWrapper())
                .Returns(_isEmptyWrapperMock.Object);

            _containsEnemyWrapperMock = new Mock<DestinationContainsEnemyMoveValidator<TestBoardEntity>.IBoardStateWrapper>();
            _wrapperMock.Setup(m => m.GetDestinationContainsEnemyMoveWrapper())
                .Returns(_containsEnemyWrapperMock.Object);
        }
        [Test]
        public void Should_return_true_for_move_to_empty_space()
        {
            var move = BoardMove.Create(BoardLocation.At(1,1), BoardLocation.At(1, 2), (int) DefaultActions.MoveOnly);
            SetupFromEntity(move, 1);
            SetupNullToEntity();
            
            Assert.True(_validator.ValidateMove(move, _wrapperMock.Object));
        }

        [Test]
        public void Should_return_true_for_move_to_enemy_piece()
        {
            var move = BoardMove.Create(BoardLocation.At(1, 1), BoardLocation.At(1, 8), (int) DefaultActions.MoveOrTake);
            SetupFromEntity(move, 1);
            SetupToEntity(move, 2);
            Assert.True(_validator.ValidateMove(move, _wrapperMock.Object));
        }

        [Test]
        public void Should_return_false_for_move_to_own_piece()
        {
            var move = BoardMove.Create(BoardLocation.At(1, 1), BoardLocation.At(5, 1), (int) DefaultActions.MoveOnly);
            SetupFromEntity(move, 1);
            SetupToEntity(move, 1);
            Assert.False(_validator.ValidateMove(move, _wrapperMock.Object));
        }

        private void SetupNullToEntity()
        {
            _isEmptyWrapperMock.Setup(m => m.GetToEntity(It.IsAny<BoardMove>()))
                .Returns((LocatedItem<TestBoardEntity>)null);
            _containsEnemyWrapperMock.Setup(m => m.GetToEntity(It.IsAny<BoardMove>()))
                .Returns((LocatedItem<TestBoardEntity>)null);
        }
        protected void SetupToEntity(BoardMove move, int owner)
        {
            var item = new LocatedItem<TestBoardEntity>(move.To, new TestBoardEntity(owner), new Paths());
            _isEmptyWrapperMock.Setup(m => m.GetToEntity(It.IsAny<BoardMove>()))
                .Returns(item);
            _containsEnemyWrapperMock.Setup(m => m.GetToEntity(It.IsAny<BoardMove>()))
                .Returns(item);
        }
        protected void SetupFromEntity(BoardMove move, int owner)
        {
            var item = new LocatedItem<TestBoardEntity>(move.From, new TestBoardEntity(owner), new Paths());
            _containsEnemyWrapperMock.Setup(m => m.GetFromEntity(It.IsAny<BoardMove>()))
                .Returns(item);
        }
    }
}