using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Game;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class MoveOnlyActionTests : ActionTestsBase<MoveOnlyAction>
    {
        [SetUp]
        public void Setup()
        {
            StateMock = new Mock<IBoardState>();
            FactoryMock = new Mock<IBoardActionFactory>();

            Action = new MoveOnlyAction(StateMock.Object, FactoryMock.Object);
        }

        [Test]
        public void Execute_clears_from_location_and_replaces_to()
        {
            var piece = new PawnEntity(Colours.White);
            SetupReturnedPiece(AnyMove.From, piece);

            Action.Execute(AnyMove);

            VerifyEntityRetrieved(AnyMove.From);
            VerifyLocationCleared(AnyMove.From);
            VerifyEntityPlaced(AnyMove.To, piece);
        }
        
    }
}