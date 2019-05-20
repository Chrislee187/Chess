using board.engine.Actions;
using board.engine.Movement;
using board.engine.Movement.Validators;
using board.engine.tests.Actions;
using NUnit.Framework;

namespace board.engine.tests.Movement
{
    [TestFixture]
    public class DestinationIsEmptyValidationTests : ValidationTestsBase
    {
        private DestinationIsEmptyValidator<TestBoardEntity> _validator;


        [SetUp]
        public void SetUp()
        {
            InitMocks();
            _validator = new DestinationIsEmptyValidator<TestBoardEntity>();
        }

        [Test]
        public void Should_return_true_for_move_to_empty_space()
        {
            var move = BoardMove.Create(BoardLocation.At(5,1), BoardLocation.At(5,2), (int)ChessMoveTypes.CastleKingSide);

            SetupToEntity(move);

            Assert.True(_validator.ValidateMove(move, RoBoardStateMock.Object));
        }

        [Test]
        public void Should_return_false_for_move_to_non_empty_space()
        {
            var move = BoardMove.Create(BoardLocation.At(1, 1), BoardLocation.At(1, 8), (int)DefaultActions.MoveOnly);

            SetupToEntity(move, new TestBoardEntity());

            Assert.False(_validator.ValidateMove(move, RoBoardStateMock.Object));
        }

    }

}