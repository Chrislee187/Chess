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
    public class UpdatePieceValidationTests
    {
        private Mock<UpdatePieceValidator<TestBoardEntity>.IBoardStateWrapper> _wrapperMock;
        private UpdatePieceValidator<TestBoardEntity> _validator;
        private Mock<DestinationIsEmptyOrContainsEnemyValidator<TestBoardEntity>.IBoardStateWrapper> _innerValidatorWrapperMock;

        [SetUp]
        public void SetUp()
        {
            _wrapperMock = new Mock<UpdatePieceValidator<TestBoardEntity>.IBoardStateWrapper>();
            _validator = new UpdatePieceValidator<TestBoardEntity>();

            _innerValidatorWrapperMock = new Mock<DestinationIsEmptyOrContainsEnemyValidator<TestBoardEntity>.IBoardStateWrapper>();
            _innerValidatorWrapperMock.Setup(m => m.GetDestinationIsEmptyWrapper())
                .Returns(new Mock<DestinationIsEmptyValidator<TestBoardEntity>.IBoardStateWrapper>().Object);
            _innerValidatorWrapperMock.Setup(m => m.GetDestinationContainsEnemyMoveWrapper())
                .Returns(new Mock<DestinationContainsEnemyMoveValidator<TestBoardEntity>.IBoardStateWrapper>().Object);
            _wrapperMock.Setup(m => m.GetDestinationIsEmptyOrContainsEnemyWrapper())
                .Returns(_innerValidatorWrapperMock.Object);
        }

        [Test]
        public void Should_return_true_for_valid_update()
        {
            var move = new BoardMove(BoardLocation.At(1, 8), BoardLocation.At(1, 8), (int)DefaultActions.UpdatePiece, new TestBoardEntity());
            SetupFromEntity(move, 1);

            Assert.True(_validator.ValidateMove(move, _wrapperMock.Object));
        }

        protected void SetupFromEntity(BoardMove move, int owner)
        {
            var item = new LocatedItem<TestBoardEntity>(move.From, new TestBoardEntity(owner), new Paths());
            _wrapperMock.Setup(m => m.GetFromEntity(It.IsAny<BoardMove>()))
                .Returns(item);
        }
    }
}