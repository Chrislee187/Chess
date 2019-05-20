using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using chess.engine.Game;
using chess.pgn;
using NUnit.Framework;

namespace chess.big.tests
{
    [TestFixture]
    public class PlayPgnFileTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(@".\PGNFiles")]
        [TestCase(@"D:\Src\PGNArchive\PGN")]
        [Explicit("WARNING: Could take a VERY long time.")]
        public void PlayAllFiles(string path)
        {
            var pgnfiles = Directory.GetFiles(path, "*.pgn", SearchOption.AllDirectories);
            var fileCount = 0;
            var gamesCount = 0;
            var fileGamesCount = 0;
            foreach (var file in pgnfiles)
            {
                fileCount++;
                if (file.Contains("FAILED")) continue;

                PlaySingleGame(PgnReader.FromFile(file));
            }

            Console.WriteLine($"Files #: {fileCount}, Total Games #: {gamesCount}");
            Assert.That(fileCount, Is.GreaterThan(0), "No files processed");
            Assert.That(gamesCount, Is.GreaterThan(0), "No games processed");
        }

        [Test]
//        [Explicit("WARNING: Could take a long time.")]
        public void Should_play_all_games_in_a_single_file()
        {
            var filename = @"D:\Src\PGNArchive\PGN\Adams\Adams.pgn";
            TestContext.Progress.WriteLine($"Playing all games from;");
            TestContext.Progress.WriteLine($"  {filename}");
            PlaySingleGame(PgnReader.FromFile(filename));
        }

        private void PlaySingleGame(PgnReader reader)
        {
            PgnGame game = null;
            ChessGame chessGame = null;
            PgnTurn lastTurn = null;
            var gameIdx = 0;
            try
            {
                var timings = new List<TimeSpan>();
                game = reader.ReadGame();
                while (game != null)
                {
                    gameIdx++;
                    chessGame = ChessFactory.NewChessGame();
                    var sw = Stopwatch.StartNew();
                    PlayTurns(game, chessGame);
                    var elapsed = sw.Elapsed;
                    TestContext.Progress.WriteAsync($"{gameIdx} : {game} ({elapsed})");
                    timings.Add(elapsed);
                    game = reader.ReadGame();
                }

                TestContext.Progress.WriteAsync($"Average playtime ({new TimeSpan(Convert.ToInt64(timings.Average(ts => ts.Ticks)))})");

            }
            catch
            {
                TestContext.Out.WriteLine($"FAILED");
                TestContext.Out.WriteLine($"Game: #{gameIdx} / {game?.ToString() ?? ""}");
                TestContext.Out.WriteLine($"Board:\n{chessGame.ToText()}");
                TestContext.Out.WriteLine($"Full PGN Text:\n{reader.LastGameText}");
                throw;
            }
        }

        private static void PlayTurns(PgnGame game, ChessGame chessGame)
        {
            PgnTurn lastTurn = null;
            try
            {
                foreach (var gameTurn in game.Turns)
                {
                    lastTurn = gameTurn;
                    if (gameTurn.White != null)
                        chessGame.Move(gameTurn.White.San);

                    if (gameTurn.Black != null)
                        chessGame.Move(gameTurn.Black.San);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Problem with: {lastTurn}", e);
            }
        }
    }
}