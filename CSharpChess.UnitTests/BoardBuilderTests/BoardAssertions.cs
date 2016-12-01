using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardBuilderTests
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
            if (!expected.Any() && !actual.Any()) return;

            var actualMoves = actual as IList<ChessMove> ?? actual.ToList();
            var startLoc = actualMoves.First().From;
            var expectedMoves = expected.Select(e => new ChessMove(startLoc, e));

            CollectionAssert.AreEquivalent(expectedMoves, actualMoves);
//            foreach (var move in expected)
//            {
//                var exp = new ChessMove(startLoc, move);
//                var moves = string.Join(", ",actualMoves.Select(a => a.ToString()));
//                Assert.False(actualMoves.All(m => m.Equals(exp)), $"Expected move '{startLoc}-{move}', not found in expected move list '{moves}'");
//            }
            Assert.That(actualMoves.Count(), Is.EqualTo(expected.Count()));
        }
    }
}