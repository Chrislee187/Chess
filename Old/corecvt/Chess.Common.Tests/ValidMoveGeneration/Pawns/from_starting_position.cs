using Chess.Common.Tests.Helpers;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using NUnit.Framework;

namespace Chess.Common.Tests.ValidMoveGeneration.Pawns
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class from_starting_position : BoardAssertions
    {
        private PawnMoveGenerator _pawnMoveGenerator;
        const int WhitePawnRank = 2;
        const int BlackPawnRank = 7;

        [SetUp]
        public void SetUp()
        {
            _pawnMoveGenerator = new PawnMoveGenerator();
        }
        [Test]
        public void can_move_one_or_two_squares()
        {
            var board = BoardBuilder.NewGame;
            foreach (var file in Info.Files)
            {
                var whitePawn = BoardLocation.At(file, WhitePawnRank);
                var blackPawn = BoardLocation.At(file, BlackPawnRank);
                var whiteExpected = BoardLocation.List($"{whitePawn.File}{whitePawn.Rank + 1}",
                    $"{whitePawn.File}{whitePawn.Rank + 2}");
                var blackExpected = BoardLocation.List($"{blackPawn.File}{blackPawn.Rank - 1}",
                    $"{blackPawn.File}{blackPawn.Rank - 2}");

                var chessMoves = new PawnMoveGenerator().All(board, whitePawn).Moves();
                AssertMovesContainsExpectedWithType(chessMoves, whiteExpected, MoveType.Move);


                AssertMovesContainsExpectedWithType(new PawnMoveGenerator().All(board, blackPawn).Moves(),
                    blackExpected, MoveType.Move);
            }
        }

        [Test]
        public void covers_nothing()
        {
            var board = BoardBuilder.NewGame;
            foreach (var file in Info.Files)
            {
                Assert.That(_pawnMoveGenerator.All(board, BoardLocation.At(file, WhitePawnRank)).Covers(), Is.Empty);
                Assert.That(_pawnMoveGenerator.All(board, BoardLocation.At(file, BlackPawnRank)).Covers(), Is.Empty);
            }
        }

        [Test]
        public void has_no_takes()
        {
            var board = BoardBuilder.NewGame;
            foreach (var file in Info.Files)
            {
                Assert.That(_pawnMoveGenerator.All(board, BoardLocation.At(file, WhitePawnRank)).Takes(), Is.Empty);
                Assert.That(_pawnMoveGenerator.All(board, BoardLocation.At(file, BlackPawnRank)).Takes(), Is.Empty);
            }
        }
    }
}
