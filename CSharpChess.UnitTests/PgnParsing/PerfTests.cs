using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CsChess.Pgn;
using CSharpChess.System;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    [TestFixture]
    public class PerfTests
    {

        private static readonly Stopwatch Stopwatch = new Stopwatch();
        private readonly IDictionary<string, TimeSpan> _gameTimes = new ConcurrentDictionary<string, TimeSpan>();

        private static readonly string Root = FullGameParsing.Root;
        [Test]
        public void PARSE_wiki_game_100_times()
        {
            ParseGameManyTimes(PgnTestGames.WikiGame, 100);
            DumpMetrics();
        }

        [Test]
        public void PLAY_wiki_game_100_times()
        {
            PlayGameManyTimes(PgnTestGames.WikiGame, 100);
            DumpMetrics();
        }

        [Test]
        public void PLAY_short_pgn_file()
        {
            PlayAllGamesFromFile($"{Root}\\short.pgn");
            DumpMetrics();
        }

        private void PlayAllGamesFromFile(string filename)
        {
            var stream = File.OpenRead(filename);

            int count = 0;

            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {
                    TimeGame($"{filename}.{count++}", game);

                    game = reader.ReadGame();
                }
            }
        }

        private void ParseGameManyTimes(string pgnGame, int iterations)
        {
            var pgnGameText = pgnGame;

            for (int i = 0; i < iterations; i++)
            {
                _gameTimes.Add($"ParseWiki{i}", Time(() => PgnGame.Parse(pgnGameText).First()));
            }
        }

        private void PlayGameManyTimes(string pgnGame, int iterations)
        {
            var pgnGameText = pgnGame;

            for (int i = 0; i < iterations; i++)
            {
                TimeGame($"PlayWiki{i}", pgnGameText);
            }
        }

        private void TimeGame(string timeKey, string pgnGameText)
        {
            _gameTimes.Add(timeKey, Time(() => PlayGame(pgnGameText)));
        }

        private void DumpMetrics()
        {
            if (_gameTimes.Any())
            {
                var minMs = _gameTimes.Values.Min().TotalMilliseconds;
                var maxMs = _gameTimes.Values.Max().TotalMilliseconds;

                var fastest = _gameTimes.First(kvp => kvp.Value.TotalMilliseconds == minMs);
                var slowest = _gameTimes.First(kvp => kvp.Value.TotalMilliseconds == maxMs);
                var averageMs = _gameTimes.Average(kvp => kvp.Value.TotalMilliseconds);

                Console.WriteLine($"Games Processed: {_gameTimes.Count()}");
                Console.WriteLine($"Slowest: {maxMs:######} : {slowest.Key}");
                Console.WriteLine($"Fastest: {minMs:######} : {fastest.Key}");
                Console.WriteLine($"Total  :  {TimeSpan.FromMilliseconds(_gameTimes.Sum(kvp => kvp.Value.TotalMilliseconds))}");
                Console.WriteLine($"Average: {averageMs:######}");
                _gameTimes.Clear();
            }
        }

        private TimeSpan Time(Action action)
        {
            Stopwatch.Reset();
            Stopwatch.Start();
            action();
            Stopwatch.Stop();
            return Stopwatch.Elapsed;
        }

        private void PlayGame(string game)
        {
            new PgnGameResolver().Resolve(PgnGame.Parse(game).Single());
        }
    }
}
