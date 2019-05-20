using board.engine.Actions;
using board.engine.Movement;
using board.engine.Movement.Validators;
using board.engine.tests.Actions;
using NUnit.Framework;

namespace board.engine.tests.Movement
{
    [TestFixture]
    public class UpdatePieceValidationTests : ValidationTestsBase
    {
        private UpdatePieceValidator<TestBoardEntity> _validator;

        [SetUp]
        public void SetUp()
        {
            InitMocks();    
            _validator = new UpdatePieceValidator<TestBoardEntity>();
        }

        [Test]
        public void Should_return_true_for_valid_update()
        {
            var move = new BoardMove(BoardLocation.At(1, 7), BoardLocation.At(1, 8), (int)DefaultActions.UpdatePiece, new TestBoardEntity());
            SetupFromEntity(move, new TestBoardEntity(1));
            SetupToEntity(move);

            Assert.True(_validator.ValidateMove(move, RoBoardStateMock.Object));
        }
    }
}