using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using board.engine;
using chess.engine.Game;
using chess.tests.utils.TestData;
using NUnit.Framework;
using PgnReader;

namespace chess.big.tests
{
    [TestFixture]
    public class PerfTests
    {
        private Dictionary<string, List<TimeSpan>> _timings;

        private readonly string _keySequential = $"{nameof(Perf_RefreshAllPaths)}.sequential";
        private readonly string _keyParallel = $"{nameof(Perf_RefreshAllPaths)}.parallel";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _timings = new Dictionary<string, List<TimeSpan>>
            {
                {_keySequential, new List<TimeSpan>()},
                {_keyParallel, new List<TimeSpan>()}
            };

            // Ensure we are as warmed up
            var warmupSeq = Play(false);
            var warmupPar = Play(true);

            // NOTE: Output from setup members does not appear in VS using the R# test runner's console
            // dotnet test from the command line will show it
            TestContext.Progress.WriteLine("Warmups");
            OutputComparison(warmupPar, warmupSeq);

            TestContext.Progress.WriteLine("Tests");
        }

        [Test]
        [Repeat(10)]
        [Explicit("Comment out this attribute and using 'dotnet test --filter chess.big.tests.PerfTests.Perf_RefreshAllPaths' to see the proper output (NUnit Test Runner issues with console output and threads)")]
        public void Perf_RefreshAllPaths()
        {
            var sequential = Play(false);
            _timings[_keySequential].Add(sequential);

            var parallel = Play(true);
            _timings[_keyParallel].Add(parallel);

            OutputComparison(parallel, sequential);
        }

        private TimeSpan Play(bool parallelise)
        {
            var previousFf = FeatureFlags.ParalleliseRefreshAllPaths;
            FeatureFlags.ParalleliseRefreshAllPaths = parallelise;
            TimeSpan elapsed;
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var pgnGame = PgnGame.ReadAllGamesFromString(WikiGame.PgnText).First();
                PlaySingleGame(pgnGame);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                elapsed = stopwatch.Elapsed;
                FeatureFlags.ParalleliseRefreshAllPaths = previousFf;
            }

            return elapsed;
        }

        private void PlaySingleGame(PgnGame game)
        {
            ChessGame chessGame = null;
            try
            {
                chessGame = ChessFactory.NewChessGame();
                PlayTurns(game, chessGame);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Game: {game?.ToString() ?? ""}");
                Console.WriteLine($"Board:\n{chessGame.ToTextBoard()}");
                Console.WriteLine($"Full PGN Text:\n{game.PgnText}");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
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
            OutputAverages(nameof(Perf_RefreshAllPaths), _keySequential, _keyParallel);
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