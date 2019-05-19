using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using board.engine;
using chess.engine.Game;
using chess.pgn;
using chess.tests.utils.TestData;
using NUnit.Framework;

namespace chess.big.tests
{
    [TestFixture]
    public class PerfTests
    {
        private Dictionary<string, List<TimeSpan>> _timings;

        private string Key_Sequential = $"{nameof(Perf_RefreshAllPaths)}.sequential";
        private string Key_Parallel = $"{nameof(Perf_RefreshAllPaths)}.parallel";
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _timings = new Dictionary<string, List<TimeSpan>>();
            _timings.Add(Key_Sequential, new List<TimeSpan>());
            _timings.Add(Key_Parallel, new List<TimeSpan>());
            
            // Ensure we are as warmed up as we can be
            var warmupSeq = Play(false);
            var warmupPar = Play(true);

            // NOTE: Output from setup members does not appear in VS using the R# test runner's console
            // dotnet test from the command line will show it
            TestContext.Progress.WriteLine("Warmups");
            OutputComparison(warmupPar, warmupSeq);
            
            TestContext.Progress.WriteLine("Tests");

        }

        [Test]
//        [Repeat(10)]
        public void Perf_RefreshAllPaths()
        {
            string Key(string k) => $"{nameof(Perf_RefreshAllPaths)}.{k}";

            var sequential = Play(false);
            _timings[Key_Sequential].Add(sequential);

            var parallel = Play(true);
            _timings[Key_Parallel].Add(parallel);

            TestContext.Progress.WriteLine($"{nameof(Perf_RefreshAllPaths)}");
            OutputComparison(parallel, sequential);
                                                          
            // Times based on initial observations rounded up to the nearest 1/4 second
            // Use a simple check to ensure we don't do something silly that radically 
            // decreases performance
            Assert.That(sequential, Is.LessThan(TimeSpan.FromSeconds(3.6)));
            Assert.That(parallel, Is.LessThan(TimeSpan.FromSeconds(1.75)));
        }

        private TimeSpan Play(bool parallelise)
        {
            var previousFF = FeatureFlags.ParalleliseRefreshAllPaths;
            FeatureFlags.ParalleliseRefreshAllPaths = parallelise;
            TimeSpan elapsed;
            var stopwatch = Stopwatch.StartNew();
            try
            {
                PlaySingleGame(PgnReader.FromString(WikiGame.PgnText));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                elapsed = stopwatch.Elapsed;
               FeatureFlags.ParalleliseRefreshAllPaths = previousFF;
            }

            return elapsed;
        }

        private void PlaySingleGame(PgnReader reader)
        {
            PgnGame game = null;
            ChessGame chessGame = null;
            PgnTurn lastTurn = null;
            var gameIdx = 0;
            try
            {
                game = reader.ReadGame();
                while (game != null)
                {
                    gameIdx++;

                    chessGame = ChessFactory.NewChessGame();
                    PlayTurns(game, chessGame);

                    game = reader.ReadGame();
                }
            }
            catch
            {
                Console.WriteLine($"Game: #{gameIdx} / {game?.ToString() ?? ""}");
                Console.WriteLine($"Board:\n{chessGame.ToText()}");
                Console.WriteLine($"Full PGN Text:\n{reader.LastGameText}");
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

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestContext.Progress.WriteLine("Averages");
            OutputAverages(nameof(Perf_RefreshAllPaths), Key_Sequential, Key_Parallel);
        }

        private void OutputAverages(string testMethodName, string keySequential, string keyParallel)
        {
            var seq = _timings[keySequential];
            var par = _timings[keyParallel];

            var parallel = new TimeSpan(Convert.ToInt64(par.Average(ts => ts.Ticks)));
            var sequential = new TimeSpan(Convert.ToInt64(seq.Average(ts => ts.Ticks)));
            TestContext.Progress.Write($"{testMethodName}: ");

            OutputComparison(parallel, sequential);
            Assert.That(parallel, Is.LessThan(sequential));
        }

        private static void OutputComparison(TimeSpan parallel, TimeSpan sequential)
        {
            TestContext.Progress.WriteLine(
                $"Parallel: {parallel} vs Non-Parallel: {sequential} - {(sequential / parallel):P2}");
        }
    }
}