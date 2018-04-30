using System;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    [TestFixture]
    public class PgnGamePlayerTests
    {
        [Test]
        public void should_()
        {
            //var gamePlayer = new PgnGamePlayer(PgnTestGames.WikiGame);

            //while (gamePlayer.InProgress)
            //{
            //    Console.WriteLine(gamePlayer.Board.ToConsoleBoard());
            //    Console.WriteLine(gamePlayer.PlayNextTurn());
            //}
        }
    }

    public class PgnGamePlayer
    {
        public string PgnText { get; }
        public bool InProgress { get; private set; }

        public PgnGamePlayer(string wikiGame)
        {
            PgnText = wikiGame;
        }

    }
}