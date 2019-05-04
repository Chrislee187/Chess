using chess.engine.Actions;
using chess.engine.Chess.Actions;
using chess.engine.Chess.Entities;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.tests.Actions;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Chess.Actions
{
    [TestFixture]
    public class EnPassantActionTests : ActionTestsBase<EnPassantAction<ChessPieceEntity>, ChessPieceEntity>
    {
        private static readonly BoardMove EnPassantMove 
            = new BoardMove(BoardLocation.At("B5"), BoardLocation.At("C6"), MoveType.TakeEnPassant);
        [SetUp]
        public void Setup()
        {
            base.SetUp();
            Action = new EnPassantAction<ChessPieceEntity>(FactoryMock.Object, StateMock.Object);
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


        [Test]
        public void Execute_empty_from_does_nothing()
        {
            StateMock.Setup(s => s.IsEmpty(It.IsAny<BoardLocation>()))
                .Returns(true);

            Action.Execute(EnPassantMove);

            VerifyEntityWasNOTRetrieved(EnPassantMove.From);

        }
    }
}