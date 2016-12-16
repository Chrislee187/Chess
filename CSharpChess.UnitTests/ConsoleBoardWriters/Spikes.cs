using System;
using System.Linq;
using CSharpChess.System.Metrics;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ConsoleBoardWriters
{
    [TestFixture, Explicit]
    public class Spikes
    {
        [Test]
        public void x()
        {
            var writer = new MediumConsoleBoard(new ChessBoard());

            writer.Build()
                .ToStrings().ToList()
                .ForEach(Console.WriteLine);

        }

        [Test]
        public void y()
        {
            ChessBoard game1, game2;

            Timers.Time("newboard.test1", () => game1 = BoardBuilder.NewGame);
            Timers.Time("newboard.test2", () => game2 = BoardBuilder.NewGame);
        }
    }
}