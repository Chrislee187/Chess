using System.Linq;
using chess.pgn.Parsing;
using chess.tests.utils.TestData;
using NUnit.Framework;

namespace chess.pgn.tests
{
    public class PgnReaderTests
    {
        [Test]
        public void Wiki_games_FromString_works()
        {
            var reader = PgnReader.FromString(
                WikiGame.PgnText + "\n" +
                WikiGame.PgnText);

            AssertWikiGame(reader);
        }
        
        [Test]
        public void Wiki_games_FromFile_works()
        {
            var reader = PgnReader.FromFile(@".\TestData\wikigame.pgn");

            AssertWikiGame(reader);
        }

        private static void AssertWikiGame(PgnReader reader)
        {
            var pgnGame = reader.ReadGame();
            Assert.NotNull(pgnGame);
            var count = 0;
            while (pgnGame != null)
            {
                count++;
                AssertWikiGameIsCorrect(pgnGame, count);
                pgnGame = reader.ReadGame();
            }

            Assert.That(count, Is.EqualTo(2), "Unexpected number of games read.");
        }
        private static void AssertWikiGameIsCorrect(PgnGame pgnGame, int gameIdx)
        {
            Assert.That(pgnGame.Event, Is.EqualTo($"F/S Return Match"), $"Event:{gameIdx}");
            Assert.That(pgnGame.Site, Is.EqualTo("Belgrade, Serbia JUG"), $"Site:{gameIdx}");
            Assert.That(pgnGame.Date.ToString(), Is.EqualTo("1992.11.04"), $"Date:{gameIdx}");
            Assert.That(pgnGame.Round, Is.EqualTo("29"), $"Round:{gameIdx}");
            Assert.That(pgnGame.White, Is.EqualTo("Fischer, Robert J."), $"White:{gameIdx}");
            Assert.That(pgnGame.Black, Is.EqualTo("Spassky, Boris V."), $"Black:{gameIdx}");
            Assert.That(pgnGame.Result, Is.EqualTo(PgnGameResult.Draw), $"Result:{gameIdx}");

            Assert.That(pgnGame.Turns.Count(), Is.EqualTo(43));
            Assert.That(pgnGame.Turns.Single(t => t.Turn == 3).Black.Comment,
                Is.EqualTo($"This opening is called the Ruy Lopez"), $":{gameIdx}");
        }
    }
}