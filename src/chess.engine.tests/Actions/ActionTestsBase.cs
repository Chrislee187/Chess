using System.ComponentModel;
using chess.engine.Actions;
using Moq;

namespace chess.engine.tests.Actions
{
    public abstract class ActionTestsBase
    {
        protected Mock<IBoardState> StateMock;
        protected Mock<IBoardActionFactory> FactoryMock;
        protected Mock<IBoardAction> BoardActionMock;
    }
}