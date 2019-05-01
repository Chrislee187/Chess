using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class TakeOnlyActionTests : ActionTestsBase<TakeOnlyAction>
    {
        private readonly ChessMove AnyTake = ChessMove.Create("D2", "D5", ChessMoveType.MoveOrTake);

        [SetUp]
        public void Setup()
        {
            StateMock = new Mock<IBoardState>();
            FactoryMock = new Mock<IBoardActionFactory>();
            BoardActionMock = new Mock<IBoardAction>();
            Action = new TakeOnlyAction(StateMock.Object, FactoryMock.Object);
        }

        [Test]
        public void Execute_clears_location_before_using_MoveOnlyAction_for_take_moves()
        {
            var piece = new RookEntity(Colours.White);
            var takePiece = new RookEntity(Colours.Black);
            StateMock.Setup(m => m.GetEntity(AnyTake.From))
                .Returns(piece);
            StateMock.Setup(m => m.GetEntity(AnyTake.To))
                .Returns(takePiece);

            FactoryMock.Setup(m => m.Create(ChessMoveType.MoveOnly, It.IsAny<IBoardState>()))
                .Returns(BoardActionMock.Object);

            Action.Execute(AnyTake);

            StateMock.Verify(s => s.ClearLocation(AnyTake.To));
            FactoryMock.Verify(m => m.Create(ChessMoveType.MoveOnly, It.IsAny<IBoardState>()));
            BoardActionMock.Verify(m => m.Execute(AnyTake));
        }
    }
}