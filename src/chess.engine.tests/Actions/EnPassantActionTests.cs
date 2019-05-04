using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class EnPassantActionTests : ActionTestsBase<EnPassantAction>
    {
        private static readonly ChessMove EnPassantMove 
            = new ChessMove(BoardLocation.At("B5"), BoardLocation.At("C6"), ChessMoveType.TakeEnPassant);
        [SetUp]
        public void Setup()
        {
            base.SetUp();
            Action = new EnPassantAction(FactoryMock.Object, StateMock.Object);
        }

        [Test]
        public void Execute_moves_friendly_pawn_and_removes_enemy_pawn()
        {
            var piece = new PawnEntity(Colours.White);
            var takenPiece = new PawnEntity(Colours.Black);

            var takenPieceLocation = EnPassantMove.To.MoveBack(Colours.White);

            SetupPieceReturn(EnPassantMove.From, piece);
            SetupPieceReturn(takenPieceLocation, takenPiece);
            SetupCreateMockActionForMoveType(DefaultActions.MoveOnly);

            Action.Execute(EnPassantMove);

            VerifyLocationWasCleared(takenPieceLocation);
            VerifyActionWasExecuted(EnPassantMove);
        }

    }
}