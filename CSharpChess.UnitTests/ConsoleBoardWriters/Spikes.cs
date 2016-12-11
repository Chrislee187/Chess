using System;
using System.Linq;
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
    }
}