using board.engine.Actions;
using board.engine.Board;
using chess.engine.Chess.Entities;
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
            ActionFactoryMock = new Mock<IBoardActionProvider<ChessPieceEntity>>();
            BoardActionMock = new Mock<IBoardAction>();
            Action = new TakeOnlyAction<ChessPieceEntity>(ActionFactoryMock.Object, StateMock.Object);
        }

        [Test]
        public void Execute_clears_location_before_using_MoveOnlyAction_for_take_moves()
        {
            var piece = new RookEntity(Colours.White);
            var takePiece = new RookEntity(Colours.Black);

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