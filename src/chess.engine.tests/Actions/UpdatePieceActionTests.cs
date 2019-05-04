using chess.engine.Actions;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class UpdatePieceActionTests : ActionTestsBase<UpdatePieceAction>
    {
        private const ChessPieceName PromotionPiece = ChessPieceName.Queen;
        private static readonly BoardMove PawnPromotionMove = new BoardMove(BoardLocation.At("B7"),BoardLocation.At("B8"), PromotionPiece);
        [SetUp]
        public void Setup()
        {
            base.SetUp();
            Action = new UpdatePieceAction(FactoryMock.Object, StateMock.Object);
        }

        [Test]
        public void Execute_moves_and_upgrades_piece()
        {
            var piece = new PawnEntity(Colours.White);
            var promotedPiece = new QueenEntity(Colours.White);
            SetupPieceReturn(PawnPromotionMove.From, piece);

            Action.Execute(PawnPromotionMove);

            VerifyLocationWasCleared(PawnPromotionMove.From);
            VerifyNewEntityWasPlaced(PawnPromotionMove.To, promotedPiece);
        }

        [Test]
        public void Execute_empty_from_does_nothing()
        {
            StateMock.Setup(s => s.IsEmpty(It.IsAny<BoardLocation>()))
                .Returns(true);

            Action.Execute(PawnPromotionMove);

            VerifyEntityWasNOTRetrieved(PawnPromotionMove.From);

        }

    }
}