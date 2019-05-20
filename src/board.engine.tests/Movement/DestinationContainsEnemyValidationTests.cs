using board.engine.Actions;
using board.engine.Movement;
using board.engine.Movement.Validators;
using board.engine.tests.Actions;
using NUnit.Framework;

namespace board.engine.tests.Movement
{
    [TestFixture]
    public class DestinationContainsEnemyValidationTests : ValidationTestsBase
    {
        private DestinationContainsEnemyMoveValidator<TestBoardEntity> _validator;

        [SetUp]
        public void SetUp()
        {
            InitMocks();    
            _validator = new DestinationContainsEnemyMoveValidator<TestBoardEntity>();
       }

        [Test]
        public void Should_return_true_when_destination_contains_enemy()
        {
            var move = BoardMove.Create(BoardLocation.At(1,1), BoardLocation.At(1, 8),
                (int) DefaultActions.MoveOrTake);

            SetupFromEntity(move, new TestBoardEntity());
            SetupToEntity(move, new TestBoardEntity(Enemy));

            Assert.True(_validator.ValidateMove(move, RoBoardStateMock.Object));
        }

        [Test]
        public void Should_return_false_when_destination_contains_friend()
        {
            var move = BoardMove.Create(BoardLocation.At(5, 8), BoardLocation.At(7, 8),
                (int) DefaultActions.MoveOrTake);

            SetupFromEntity(move, new TestBoardEntity());
            SetupToEntity(move, new TestBoardEntity());

            Assert.False(_validator.ValidateMove(move, RoBoardStateMock.Object));
        }

        [Test]
        public void Should_return_false_when_destination_is_empty()
        {
            var move = BoardMove.Create(BoardLocation.At(5, 8), BoardLocation.At(7, 8),
                (int)DefaultActions.MoveOrTake);

            SetupFromEntity(move, new TestBoardEntity());
            SetupToEntity(move);

            Assert.False(_validator.ValidateMove(move, RoBoardStateMock.Object));
        }
    }
}