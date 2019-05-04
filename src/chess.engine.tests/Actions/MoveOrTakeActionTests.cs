using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Chess.Entities;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
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
            FactoryMock = new Mock<IBoardActionFactory<ChessPieceEntity>>();
            BoardActionMock = new Mock<IBoardAction>();

            Action = new MoveOrTakeAction<ChessPieceEntity>(FactoryMock.Object, StateMock.Object);
        }

        [Test]
        public void Execute_uses_MoveOnlyAction_for_normal_moves()
        {
            var piece = new RookEntity(Colours.White);

            SetupPieceReturn(AnyMove.From, piece);
            SetupCreateMockActionForMoveType(DefaultActions.MoveOnly);

            Action.Execute(AnyMove);

            VerifyActionWasCreated(DefaultActions.MoveOnly);
            VerifyActionWasExecuted(AnyMove);
        }


        [Test]
        public void Execute_uses_TakeOnlyAction_for_take_moves()
        {
            var piece = new RookEntity(Colours.White);
            var takePiece = new RookEntity(Colours.Black);

            SetupPieceReturn(AnyTake.From, piece);
            SetupPieceReturn(AnyTake.To, takePiece);
            SetupCreateMockActionForMoveType(DefaultActions.TakeOnly);

            Action.Execute(AnyTake);

            VerifyActionWasCreated(DefaultActions.TakeOnly);
            VerifyActionWasExecuted(AnyTake);
        }
    }
}