using System;
using System.Linq;
using Chess.Common.Tests.Helpers;
using CSharpChess;
using CSharpChess.System.Metrics;
using NUnit.Framework;

namespace Chess.Common.Tests.ConsoleBoardWriters
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