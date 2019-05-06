using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Chess.Entities;
using chess.engine.Entities;
using chess.engine.Game;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class TakeOnlyActionTests : ActionTestsBase<TakeOnlyAction<ChessPieceEntity>, ChessPieceEntity>
    {
        [SetUp]
        public void Setup()
        {
            StateMock = new Mock<IBoardState<ChessPieceEntity>>();
            FactoryMock = new Mock<IBoardActionFactory<ChessPieceEntity>>();
            BoardActionMock = new Mock<IBoardAction>();
            Action = new TakeOnlyAction<ChessPieceEntity>(FactoryMock.Object, StateMock.Object);
        }

        [Test]
        public void Execute_clears_location_before_using_MoveOnlyAction_for_take_moves()
        {
            var piece = new RookEntity(Colours.White);
            var takePiece = new RookEntity(Colours.Black);

            SetupPieceReturn(AnyTake.From, piece);
            SetupPieceReturn(AnyTake.To, takePiece);
            SetupCreateMockActionForMoveType(DefaultActions.MoveOnly);

            Action.Execute(AnyTake);

            VerifyLocationWasCleared(AnyTake.To);
            VerifyActionWasCreated(DefaultActions.MoveOnly);
            VerifyActionWasExecuted(AnyTake);
        }
    }
}