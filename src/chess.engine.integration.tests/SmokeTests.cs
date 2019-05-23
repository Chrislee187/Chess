using System;
using chess.engine.Game;
using chess.tests.utils.TestData;
using NUnit.Framework;

namespace chess.engine.integration.tests
{
    [TestFixture]
    public class SmokeTests
    {
        [Test]
        public void Should_play_the_wiki_game_with_san_moves()
        {
            var game = ChessFactory.NewChessGame(ChessFactory.LoggerType.Null);
            var moveIdx = 0;
            foreach (var move in WikiGame.Moves)
            {
                TestContext.Progress.WriteLine($"Move #{++moveIdx}: {move}");
                var msg = game.Move(move);
                TestContext.Progress.WriteLine(game.CheckState + " - " + game.CurrentPlayer + " to move.");
                TestContext.Progress.WriteLine(game.ToTextBoard());
                if (!string.IsNullOrEmpty(msg))
                {
                    if (msg.Contains("Error:")) Assert.Fail($"Error: {msg}");
                    TestContext.Progress.WriteLine(msg);
                }
            }

            TestContext.Progress.WriteLine("GAME OVER!");
        }

    }
}


