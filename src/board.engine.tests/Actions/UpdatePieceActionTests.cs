using board.engine.Actions;
using board.engine.Movement;
using Moq;
using NUnit.Framework;

namespace board.engine.tests.Actions
{
    [TestFixture]
    public class UpdatePieceActionTests : ActionTestsBase<UpdatePieceAction<TestBoardEntity>, TestBoardEntity>
    {
        private static readonly TestBoardEntity TestUpdateEntity = new TestBoardEntity();

        private static readonly BoardMove PawnPromotionMove
            = new BoardMove(BoardLocation.At(2,7), BoardLocation.At(2, 8), (int)DefaultActions.UpdatePiece,
                TestUpdateEntity);


        [SetUp]
        public void Setup()
        {
            SetUp();
            Action = new UpdatePieceAction<TestBoardEntity>(
                EntityFactoryMock.Object, 
                ActionFactoryMock.Object, 
                StateMock.Object);
        }

        [Test]
        public void Execute_moves_and_upgrades_piece()
        {
            var piece = new TestBoardEntity();
            var promotedPiece = TestUpdateEntity;
            EntityFactoryMock.Setup(f => f.Create(It.IsAny<object>())).Returns(promotedPiece);
            SetupLocationReturn(PawnPromotionMove.From, piece);
            SetupPromotionPiece(promotedPiece);
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