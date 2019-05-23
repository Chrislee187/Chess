using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using chess.engine.Game;
using chess.pgn;
using chess.tests.utils.TestData;
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

        [Test(Description = "Best Average, inVS: 0.35, inConsole (RELEASE): 0.23 ")]
        [Explicit("WARNING: Could take a long time.")] // NEVER COMMIT THIS !!!!!!!!!!!!!!!!!!!!!!!!!
        public void Measure_parse_game_time_100_games()
        {
            var filename = @"D:\Src\PGNArchive\PGN\Modern100.pgn";
            TestContext.Progress.WriteLine($"Playing all games from;");
            TestContext.Progress.WriteLine($"  {filename}");
            PlayAllGames(PgnReader.FromFile(filename));
            TestContext.Progress.WriteLine($"  {filename} complete!");
        }

        [Test(Description = "Best Average, inVS: 0.35, inConsole (RELEASE): 0.23 ")]
        [Explicit("WARNING: Could take a long time.")] // NEVER COMMIT THIS !!!!!!!!!!!!!!!!!!!!!!!!!
        public void Measure_parse_wiki_game_time_100_games()
        {
            var times = new List<TimeSpan>();
            for (int i = 0; i < 100; i++)
            {
                var sw = Stopwatch.StartNew();
                PlayAllGames(PgnReader.FromString(WikiGame.PgnText));
                times.Add(sw.Elapsed);
            }

            TestContext.Progress.WriteLine($"Average Game Parse TIme: { new TimeSpan(Convert.ToInt64(times.Average(ts => ts.Ticks)))}");
        }
//        [TestCase(@".\PGNFiles")]
        [TestCase(@"D:\Src\PGNArchive\PGN")]
        [Explicit("WARNING: Could take a VERY long time.")]// NEVER COMMIT THIS !!!!!!!!!!!!!!!!!!!!!!!!!
        public void PlayAllFiles(string path)
        {
            var pgnfiles = Directory.GetFiles(path, "*.pgn", SearchOption.AllDirectories);
            var fileCount = 0;
            var gamesCount = 0;
            var sw = Stopwatch.StartNew();
            var fileTimes = new List<TimeSpan>();

            // NOTE: As this loop includes file access care must be taken with current VS/R#dotnet type setups
            // if they run tests in the background they can lock the files and cause weird errors here.
            // TODO: Change these long running tests to a console application
            pgnfiles.AsParallel().ForAll(file =>
                {
                    TestContext.Progress.WriteLine("*************************************");
                    TestContext.Progress.WriteLine($"*** starting file: {file}");
                    TestContext.Progress.WriteLine("*************************************");
                    var fileSw = new Stopwatch();
                    if (file.Contains("FAILED")) return;
                    fileCount++;

                    var text = File.ReadAllText(file);
                    PlayAllGames(PgnReader.FromString(text));
                    fileSw.Stop();
                    fileTimes.Add(fileSw.Elapsed);
                    TestContext.Progress.WriteLine("*************************************");
                    TestContext.Progress.WriteLine($"*** finished file: {file} in {fileSw.Elapsed}");
                    TestContext.Progress.WriteLine("*************************************");
                });
            sw.Stop();
            Console.WriteLine($"Files #: {fileCount}/{pgnfiles.Length}, Total Games #: {gamesCount}");
            Console.Write($"Total time: {sw.Elapsed}, Avg. File Parse Time: {sw.Elapsed / pgnfiles.Length}");
            Assert.That(fileCount, Is.GreaterThan(0), "No files processed");
            Assert.That(gamesCount, Is.GreaterThan(0), "No games processed");
        }

        [Test]
        [Explicit("WARNING: Could take a VERY long time.")] // NEVER COMMIT THIS !!!!!!!!!!!!!!!!!!!!!!!!!
        public void Should_play_all_games_in_a_single_file()
        {
            //  Last Test: 19/05/19 - 58.7377 Minutes - 3081 games    Average playtime (00:00:01.1312775) (DEBUG)
            //  Last Test: 21/05/19 - 15.5203 Minutes - 3081 games    Average playtime (00:00:00.2983940)
            //            var filename = @"D:\Src\PGNArchive\PGN\Adams\Adams.pgn";

            //  Last Test: 19/05/19 - 24.0282 Minutes - 1250 games    Average playtime (00:00:01.1436758) (DEBUG)
            //var filename = @"D:\Src\PGNArchive\PGN\Akobian\Akobian.pgn"; 

            //  Last Test: 19/05/19 - 29.0058 Minutes - 1880 games    Average playtime (00:00:00.9179160)(RELEASE)
            // var filename = @"D:\Src\PGNArchive\PGN\Akopian\Akopian.pgn";

            //  Last Test: 19/05/19 - 12.1338 - 776  Average playtime (00:00:00.9298538) (RELEASE)
            //  Last Test: 22/05/19 -  3.8212 - 776  Average playtime (00:00:00.2911059)
            //            var filename = @"D:\Src\PGNArchive\PGN\Alburt\Alburt.pgn";

            var filename = @"D:\Src\PGNArchive\PGN\Modern\Modern.pgn";


            TestContext.Progress.WriteLine($"Playing all games from;");
            TestContext.Progress.WriteLine($"  {filename}");
            PlayAllGames(PgnReader.FromFile(filename));
            TestContext.Progress.WriteLine($"  {filename} complete!");
        }

        // TODO: Add an option to parallelise this
        private void PlayAllGames(PgnReader reader)
        {
            var loggerType = ChessFactory.LoggerType.Null;
            PgnGame game = null;
            ChessGame chessGame = null;
            var gameIdx = 0;
            try
            {
                var timings = new List<TimeSpan>();
                game = reader.ReadGame();
                while (game != null)
                {
                    gameIdx++;
                    chessGame = ChessFactory.NewChessGame(loggerType);
                    var sw = Stopwatch.StartNew();
                    PlayTurns(game, chessGame);
                    var elapsed = sw.Elapsed;
                    var desc = $"{game.Event} {game.Round} {game.White} vs {game.Black} {game.Result}";
                    TestContext.Progress.WriteAsync($"{gameIdx} : {desc} ({elapsed})");
                    timings.Add(elapsed);
                    game = reader.ReadGame();
                }

                TestContext.Progress.WriteAsync($"Average playtime ({new TimeSpan(Convert.ToInt64(timings.Average(ts => ts.Ticks)))})");

            }
            catch
            {
                TestContext.Out.WriteLine($"FAILED");
                TestContext.Out.WriteLine($"Game: #{gameIdx} / {game?.ToString() ?? ""}");
                TestContext.Out.WriteLine($"Board:\n{chessGame.ToTextBoard()}");
                TestContext.Out.WriteLine($"Full PGN Text:\n{reader.LastGameText}");
                throw;
            }

            TestContext.WriteLine("Finished");
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