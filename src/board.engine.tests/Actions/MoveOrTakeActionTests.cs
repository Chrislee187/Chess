using board.engine.Actions;
using board.engine.Board;
using Moq;
using NUnit.Framework;

namespace board.engine.tests.Actions
{
    [TestFixture]
    public class MoveOrTakeActionTests : ActionTestsBase<MoveOrTakeAction<TestBoardEntity>, TestBoardEntity>
    {
        [SetUp]
        public void Setup()
        {
            StateMock = new Mock<IBoardState<TestBoardEntity>>();
            ActionFactoryMock = new Mock<IBoardActionProvider<TestBoardEntity>>();
            BoardActionMock = new Mock<IBoardAction>();

            Action = new MoveOrTakeAction<TestBoardEntity>(ActionFactoryMock.Object, StateMock.Object);
        }

        [Test]
        public void Execute_uses_MoveOnlyAction_for_normal_moves()
        {
            var piece = new TestBoardEntity();

            SetupLocationReturn(AnyMove.From, piece);
            SetupLocationReturn(AnyMove.To, null);
            SetupMockActionForMoveType((int) DefaultActions.MoveOnly);
            SetupStateIsEmpty(AnyMove.To, true);

            Action.Execute(AnyMove);

            VerifyActionWasCreated((int) DefaultActions.MoveOnly);
            VerifyActionWasExecuted(AnyMove);
        }


        [Test]
        public void Execute_uses_TakeOnlyAction_for_take_moves()
        {
            var piece = new TestBoardEntity();
            var takePiece = new TestBoardEntity();

            SetupLocationReturn(AnyTake.From, piece);
            SetupLocationReturn(AnyTake.To, takePiece);
            SetupStateIsEmpty(It.IsAny<BoardLocation>(), false);
            SetupMockActionForMoveType((int)DefaultActions.TakeOnly);

            Action.Execute(AnyTake);

            VerifyActionWasCreated((int)DefaultActions.TakeOnly);
            VerifyActionWasExecuted(AnyTake);
        }
    }
}