using System;
using System.Diagnostics;
using board.engine;
using board.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.tests.utils.TestData;
using NUnit.Framework;

namespace chess.engine.tests.Game
{
    [TestFixture]
    public class SmokeTests
    {
        [Test]
        public void Should_play_the_wiki_game_with_san_moves()
        {
            // TODO: Why dooes this sometimes take 30sec'ish and other 7-8?
            var game = ChessFactory.NewChessGame(ChessFactory.LoggerType.Null);
            var moveIdx = 0;
            foreach (var move in WikiGame.Moves)
            {
                Console.WriteLine($"Move #{++moveIdx}: {move}");
                var msg = game.Move(move);
                Console.WriteLine(game.CheckState + " - " + game.CurrentPlayer + " to move.");
                Console.WriteLine(game.ToTextBoard());
                if (!string.IsNullOrEmpty(msg))
                {
                    if (msg.Contains("Error:")) Assert.Fail($"Error: {msg}");
                    Console.WriteLine(msg);
                }
            }

            Console.WriteLine("GAME OVER!");
        }

    }
}


