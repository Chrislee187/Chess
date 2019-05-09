using board.engine;
using board.engine.Actions;
using board.engine.Board;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class MoveOrTakeActionTests : ActionTestsBase<MoveOrTakeAction<ChessPieceEntity>, ChessPieceEntity>
    {
        [SetUp]
        public void Setup()
        {
            StateMock = new Mock<IBoardState<ChessPieceEntity>>();
            ActionFactoryMock = new Mock<IBoardActionFactory<ChessPieceEntity>>();
            BoardActionMock = new Mock<IBoardAction>();

            Action = new MoveOrTakeAction<ChessPieceEntity>(ActionFactoryMock.Object, StateMock.Object);
        }

        [Test]
        public void Execute_uses_MoveOnlyAction_for_normal_moves()
        {
            var piece = new RookEntity(Colours.White);

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
            var piece = new RookEntity(Colours.White);
            var takePiece = new RookEntity(Colours.Black);

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