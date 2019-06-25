using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using board.engine.tests.Actions;
using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.Game;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class EnPassantActionTests : ActionTestsBase<EnPassantAction, ChessPieceEntity>
    {
        private static readonly BoardMove EnPassantMove 
            = new BoardMove("B5".ToBoardLocation(), "C6".ToBoardLocation(), (int) ChessMoveTypes.TakeEnPassant);
        [SetUp]
        public void Setup()
        {
            base.SetUp();
            Action = new EnPassantAction(ActionFactoryMock.Object, StateMock.Object);
        }

        [Test]
        public void Execute_moves_friendly_pawn_and_removes_enemy_pawn()
        {
            var piece = new PawnEntity(Colours.White);
            var takenPiece = new PawnEntity(Colours.Black);

            var takenPieceLocation = EnPassantMove.To.MoveBack(Colours.White);

            SetupLocationReturn(EnPassantMove.From, piece);
            SetupLocationReturn(takenPieceLocation, takenPiece);
            SetupMockActionForMoveType((int)DefaultActions.MoveOnly);

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