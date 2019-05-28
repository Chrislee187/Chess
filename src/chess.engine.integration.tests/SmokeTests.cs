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
        [Test]
        public void Should_play_to_fools_mate()
        {
            var moves = new[] {"f3", "e5", "g4", "Qh4"};
            var game = ChessFactory.NewChessGame(ChessFactory.LoggerType.Null);
            foreach (var move in moves)
            {
                var msg = game.Move(move);
                if (!string.IsNullOrEmpty(msg))
                {
                    if (msg.Contains("Error:")) Assert.Fail($"Error: {msg}");
                }
            }

            Assert.That(game.CheckState, Is.EqualTo(GameCheckState.WhiteCheckmated));
        }
    }
}


