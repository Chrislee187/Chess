using chess.engine.Actions;
using chess.engine.Chess.Actions;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces;
using chess.engine.tests.Actions;
using NUnit.Framework;

namespace chess.engine.tests.Chess.Actions
{
    [TestFixture]
    public class CastleActionTests : ActionTestsBase<CastleAction>
    {
        private const bool KingSide = true;
        private const bool QueenSide = false;
        [SetUp]
        public void Setup()
        {
            base.SetUp();
            Action = new CastleAction(FactoryMock.Object, StateMock.Object);
        }

        [TestCase(Colours.White, KingSide)]
        [TestCase(Colours.Black, QueenSide)]
        public void Execute_can_castle_both_side(Colours colour, bool side)
        {
            int rank = colour == Colours.White ? 1 : 8;

            var king = new KingEntity(colour);
            var kingStartFile = King.StartPositionFor(colour).File;
            var kingDestinationFile = side ? ChessFile.G : ChessFile.C;
            var kingStartLoc = BoardLocation.At($"{kingStartFile}{rank}");
            var kingDestination = BoardLocation.At($"{kingDestinationFile}{rank}");

            var rook = new RookEntity(colour);
            var rookStartFile = side ? ChessFile.H : ChessFile.A;
            var rookDestinationFile = side ? ChessFile.F : ChessFile.D;
            var rookStart = BoardLocation.At($"{rookStartFile}{rank}");
            var rookDestination = BoardLocation.At($"{rookDestinationFile}{rank}");

            var actualKingMove = new BoardMove(kingStartLoc, kingDestination, MoveType.MoveOnly);
            var actualRookMove = new BoardMove(rookStart, rookDestination, MoveType.MoveOnly);

            SetupPieceReturn(kingStartLoc, king);
            SetupPieceReturn(rookStart, rook);
            SetupCreateMockActionForMoveType(DefaultActions.MoveOnly);
            Action.Execute(actualKingMove);

            VerifyActionWasCreated(DefaultActions.MoveOnly);
            VerifyActionWasExecuted(actualKingMove);
            VerifyActionWasExecuted(actualRookMove);
        }
    }
}