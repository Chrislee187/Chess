using System.ComponentModel;
using chess.engine.Actions;
using chess.engine.Movement;
using Moq;

namespace chess.engine.tests.Actions
{
    public abstract class ActionTestsBase<T> where T : IBoardAction
    {
        protected readonly ChessMove AnyMove = ChessMove.Create("D2", "D4", ChessMoveType.MoveOnly);
        protected readonly ChessMove AnyTake = ChessMove.Create("D2", "D5", ChessMoveType.MoveOrTake);


        protected Mock<IBoardState> StateMock = new Mock<IBoardState>();
        protected Mock<IBoardActionFactory> FactoryMock = new Mock<IBoardActionFactory>();
        protected Mock<IBoardAction> BoardActionMock = new Mock<IBoardAction>();

        protected T Action;
    }
}