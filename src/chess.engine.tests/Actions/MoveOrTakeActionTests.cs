using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class MoveOrTakeActionTests : ActionTestsBase<MoveOrTakeAction>
    {
        [SetUp]
        public void Setup()
        {
            StateMock = new Mock<IBoardState>();
            FactoryMock = new Mock<IBoardActionFactory>();
            BoardActionMock = new Mock<IBoardAction>();

            Action = new MoveOrTakeAction(StateMock.Object, FactoryMock.Object);
        }

        [Test]
        public void Execute_uses_MoveOnlyAction_for_normal_moves()
        {
            var piece = new RookEntity(Colours.White);

            SetupReturnedPiece(AnyMove.From, piece);
            SetupActionCreateForMockAction(ChessMoveType.MoveOnly);

            Action.Execute(AnyMove);

            VerifyActionWasCreated(ChessMoveType.MoveOnly);
            VerifyActionWasExecuted(AnyMove);
        }


        [Test]
        public void Execute_clears_from_location_before_using_MoveOnlyAction_for_take_moves()
        {
            var piece = new RookEntity(Colours.White);
            var takePiece = new RookEntity(Colours.Black);

            SetupReturnedPiece(AnyTake.From, piece);
            SetupReturnedPiece(AnyTake.To, takePiece);
            SetupActionCreateForMockAction(ChessMoveType.MoveOnly);

            Action.Execute(AnyTake);

            VerifyLocationCleared(AnyTake.To);
            VerifyActionWasCreated(ChessMoveType.MoveOnly);
            VerifyActionWasExecuted(AnyTake);
        }
    }
}