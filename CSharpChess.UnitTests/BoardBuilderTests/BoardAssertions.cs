using System;
using System.Linq;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardBuilderTests
{
    public class BoardAssertions
    {
        protected static void AssertNewGameBoard(ChessBoard board)
        {
            var view = new OneCharBoard(board);
            var ranks = view.Ranks.ToList();
            ranks.ForEach(Console.WriteLine);
            Assert.That(ranks[7], Is.EqualTo("rnbqkbnr"));
            Assert.That(ranks[6], Is.EqualTo("pppppppp"));
            Assert.That(ranks[5], Is.EqualTo("........"));
            Assert.That(ranks[4], Is.EqualTo("........"));
            Assert.That(ranks[3], Is.EqualTo("........"));
            Assert.That(ranks[2], Is.EqualTo("........"));
            Assert.That(ranks[1], Is.EqualTo("PPPPPPPP"));
            Assert.That(ranks[0], Is.EqualTo("RNBQKBNR"));
        }
    }
}