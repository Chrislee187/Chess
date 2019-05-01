using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
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
            StateMock.Setup(m => m.GetEntity(AnyMove.From))
                .Returns(piece);

            Action.Execute(AnyMove);

            // Gets source piece
            StateMock.Verify(m => m.GetEntity(
                It.Is<BoardLocation>(l => l.Equals(AnyMove.From))
            ), Times.Once);

            // Clears source piece's location
            StateMock.Verify(m => m.ClearLocation(
                It.Is<BoardLocation>(l => l.Equals(AnyMove.From))
            ), Times.Once);

            // Puts source piece in destination
            StateMock.Verify(m => m.SetEntity(
                It.Is<BoardLocation>(location => location.Equals(AnyMove.To)), 
                It.Is<ChessPieceEntity>(p => p.EntityType == piece.EntityType && p.Player == piece.Player)
            ), Times.Once);
        }
        
    }
}