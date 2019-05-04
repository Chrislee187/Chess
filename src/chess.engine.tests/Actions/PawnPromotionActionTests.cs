using chess.engine.Actions;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class PawnPromotionActionTests : ActionTestsBase<PawnPromotionAction>
    {
        private const ChessPieceName PromotionPiece = ChessPieceName.Queen;
        private static readonly ChessMove PawnPromotionMove = new ChessMove(BoardLocation.At("B7"),BoardLocation.At("B8"), PromotionPiece);
        [SetUp]
        public void Setup()
        {
            base.SetUp();
            Action = new PawnPromotionAction(FactoryMock.Object, StateMock.Object);
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

    }
}