using board.engine.Actions;
using board.engine.Board;
using board.engine.tests.utils;
using Moq;
using NUnit.Framework;

namespace board.engine.tests.Actions
{
    [TestFixture]
    public class TakeOnlyActionTests : ActionTestsBase<TakeOnlyAction<TestBoardEntity>, TestBoardEntity>
    {
        [SetUp]
        public void Setup()
        {
            StateMock = new Mock<IBoardState<TestBoardEntity>>();
            ActionFactoryMock = new Mock<IBoardActionProvider<TestBoardEntity>>();
            BoardActionMock = new Mock<IBoardAction>();
            Action = new TakeOnlyAction<TestBoardEntity>(ActionFactoryMock.Object, StateMock.Object);
        }

        [Test]
        public void Execute_clears_location_before_using_MoveOnlyAction_for_take_moves()
        {
            var piece = new TestBoardEntity();
            var takePiece = new TestBoardEntity();

            SetupLocationReturn(AnyTake.From, piece);
            SetupLocationReturn(AnyTake.To, takePiece);
            SetupMockActionForMoveType((int)DefaultActions.MoveOnly);

            Action.Execute(AnyTake);

            VerifyLocationWasCleared(AnyTake.To);
            VerifyActionWasCreated((int)DefaultActions.MoveOnly);
            VerifyActionWasExecuted(AnyTake);
        }
    }
}