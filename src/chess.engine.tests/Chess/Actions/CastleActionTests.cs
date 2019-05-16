using board.engine.Actions;
using board.engine.Movement;
using board.engine.tests.Actions;
using chess.engine.Chess.Actions;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Pieces;
using chess.engine.Extensions;
using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.tests.Chess.Actions
{
    [TestFixture]
    public class CastleActionTests : ActionTestsBase<CastleAction<ChessPieceEntity>, ChessPieceEntity>
    {
        private const bool KingSide = true;
        private const bool QueenSide = false;
        [SetUp]
        public void Setup()
        {
            SetUp();
            Action = new CastleAction<ChessPieceEntity>(ActionFactoryMock.Object, StateMock.Object);
        }

        [TestCase(Colours.White, KingSide)]
        [TestCase(Colours.Black, QueenSide)]
        public void Execute_can_castle_both_side(Colours colour, bool side)
        {
            int rank = colour == Colours.White ? 1 : 8;

            var king = new KingEntity(colour);
            var kingStartFile = King.StartPositionFor(colour).X;
            var kingDestinationFile = side ? ChessFile.G : ChessFile.C;
            var kingStartLoc = $"{kingStartFile}{rank}".ToBoardLocation();
            var kingDestination = $"{kingDestinationFile}{rank}".ToBoardLocation();

            var rook = new RookEntity(colour);
            var rookStartFile = side ? ChessFile.H : ChessFile.A;
            var rookDestinationFile = side ? ChessFile.F : ChessFile.D;
            var rookStart = $"{rookStartFile}{rank}".ToBoardLocation();
            var rookDestination = $"{rookDestinationFile}{rank}".ToBoardLocation();

            var actualKingMove = new BoardMove(kingStartLoc, kingDestination, (int) DefaultActions.MoveOnly);
            var actualRookMove = new BoardMove(rookStart, rookDestination, (int)DefaultActions.MoveOnly);

            SetupLocationReturn(kingStartLoc, king);
            SetupLocationReturn(rookStart, rook);
            SetupMockActionForMoveType((int)DefaultActions.MoveOnly);
            Action.Execute(actualKingMove);

            VerifyActionWasCreated((int)DefaultActions.MoveOnly);
            VerifyActionWasExecuted(actualKingMove);
            VerifyActionWasExecuted(actualRookMove);
        }
    }
}