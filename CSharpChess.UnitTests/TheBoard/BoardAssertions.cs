using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.TheBoard
{
    public class BoardAssertions
    {
        protected static void AssertNewGameBoard(ChessBoard board)
        {
            var ranks = DumpBoardToConsole(board);
            Assert.That(ranks[7], Is.EqualTo("rnbqkbnr"));
            Assert.That(ranks[6], Is.EqualTo("pppppppp"));
            Assert.That(ranks[5], Is.EqualTo("........"));
            Assert.That(ranks[4], Is.EqualTo("........"));
            Assert.That(ranks[3], Is.EqualTo("........"));
            Assert.That(ranks[2], Is.EqualTo("........"));
            Assert.That(ranks[1], Is.EqualTo("PPPPPPPP"));
            Assert.That(ranks[0], Is.EqualTo("RNBQKBNR"));
        }

        private static List<string> DumpBoardToConsole(ChessBoard board)
        {
            var view = new OneCharBoard(board);
            var ranks = view.Ranks.ToList();
            ranks.ForEach(Console.WriteLine);
            return ranks;
        }

        protected static void AssertExpectedMoves(IEnumerable<BoardLocation> expected, IEnumerable<ChessMove> actual)
        {
            var expectedLocations = expected as IList<BoardLocation> ?? expected.ToList();
            var acutalMoves = actual as IList<ChessMove> ?? actual.ToList();

            if (!expectedLocations.Any() && !acutalMoves.Any()) return;

            var actualMoves = actual as IList<ChessMove> ?? acutalMoves.ToList();
            var startLoc = actualMoves.First().From;
            var expectedMoves = expectedLocations.Select(e => new ChessMove(startLoc, e, MoveType.Unknown));

            CollectionAssert.AreEquivalent(expectedMoves, actualMoves);

            Assert.That(actualMoves.Count(), Is.EqualTo(expectedLocations.Count()));
        }
    }
}