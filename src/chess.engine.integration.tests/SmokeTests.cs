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
            foreach (var move in WikiGame.Moves)
            {
                var msg = game.Move(move);
                if (!string.IsNullOrEmpty(msg))
                {
                    if (msg.Contains("Error:")) Assert.Fail($"Error: {msg}");
                }
            }
        }

    }
}


