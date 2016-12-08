using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Pawns
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class from_starting_position : BoardAssertions
    {
        const int WhitePawnRank = 2;
        const int BlackPawnRank = 7;

        [Test]
        public void can_move_one_or_two_squares()
        {
            var board = BoardBuilder.NewGame;
            foreach (var file in Chess.Files)
            {
                var whitePawn = BoardLocation.At(file, WhitePawnRank);
                var blackPawn = BoardLocation.At(file, BlackPawnRank);
                var whiteExpected = BoardLocation.List($"{whitePawn.File}{whitePawn.Rank + 1}", $"{whitePawn.File}{whitePawn.Rank + 2}");
                var blackExpected = BoardLocation.List($"{blackPawn.File}{blackPawn.Rank - 1}", $"{blackPawn.File}{blackPawn.Rank - 2}");

                AssertMovesContainsExpectedWithType(new PawnValidMoveGenerator().ValidMoves(board, whitePawn), whiteExpected, MoveType.Move);
                AssertMovesContainsExpectedWithType(new PawnValidMoveGenerator().ValidMoves(board, blackPawn), blackExpected, MoveType.Move);
            }
        }
    }
}