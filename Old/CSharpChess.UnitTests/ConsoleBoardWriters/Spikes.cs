using System;
using System.Linq;
using CsChess;
using CSharpChess.System.Metrics;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ConsoleBoardWriters
{
    [TestFixture, Explicit]
    public class Spikes
    {
        [Test]
        public void X()
        {
            var writer = new MediumConsoleBoard(new Board());

            writer.Build()
                .ToStrings().ToList()
                .ForEach(Console.WriteLine);

        }

        [Test]
        public void Y()
        {
            // ReSharper disable once NotAccessedVariable
            Board game1, game2;

            Timers.Time("newboard.test1", () => game1 = BoardBuilder.NewGame);
            Timers.Time("newboard.test2", () => game2 = BoardBuilder.NewGame);
        }
    }
}