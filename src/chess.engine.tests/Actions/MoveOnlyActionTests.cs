using board.engine;
using board.engine.Actions;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class MoveOnlyActionTests : ActionTestsBase<MoveOnlyAction<TestBoardEntity>, TestBoardEntity>
    {
        [SetUp]
        public void Setup()
        {
            base.SetUp();
            Action = new MoveOnlyAction<TestBoardEntity>(ActionFactoryMock.Object, StateMock.Object);
        }

        [Test]
        public void Execute_clears_from_location_and_replaces_to()
        {
            var piece = new TestBoardEntity();
            SetupLocationReturn(AnyMove.From, piece);

            Action.Execute(AnyMove);

            VerifyEntityWasRetrieved(AnyMove.From);
            VerifyLocationWasCleared(AnyMove.From);
            VerifyEntityWasPlaced(AnyMove.To, piece);
        }

        [Test]
        public void Execute_empty_from_does_nothing()
        {
            StateMock.Setup(s => s.IsEmpty(It.IsAny<BoardLocation>()))
                .Returns(true);

            Action.Execute(AnyMove);

            VerifyEntityWasNOTRetrieved(AnyMove.From);

        }


    }
}