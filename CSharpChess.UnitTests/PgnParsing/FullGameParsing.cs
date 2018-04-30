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
    public class FullGameParsing
    {
        [Test]
        public void can_parse_wiki_sample_game()
        {
            var pgnGameText = PgnTestGames.WikiGame;

            var pgnGame = PgnGame.Parse(pgnGameText).First();

            Assert.That(pgnGame.Event, Is.EqualTo("F/S Return Match"));
            Assert.That(pgnGame.Site, Is.EqualTo("Belgrade, Serbia JUG"));
            Assert.That(pgnGame.Date.ToString(), Is.EqualTo("1992.11.04"));
            Assert.That(pgnGame.Round, Is.EqualTo(29));
            Assert.That(pgnGame.White, Is.EqualTo("Fischer, Robert J."));
            Assert.That(pgnGame.Black, Is.EqualTo("Spassky, Boris V."));
            Assert.That(pgnGame.Result, Is.EqualTo(ChessGameResult.Draw));

            Assert.That(pgnGame.TurnQueries.Count(), Is.EqualTo(43));

            Assert.That(pgnGame.TurnQueries.Count(), Is.EqualTo(43));
        }

        [Test]
        public void can_parse_lots_of_games()
        {
            
            var pgnText = File.ReadAllText(@"C:\Src\Info\CSharpChess.UnitTests\bin\Debug\spassky.pgn");

            var pgnGames = PgnGame.Parse(pgnText).ToList();

            Assert.That(pgnGames.Count(), Is.GreaterThan(0));
            Console.WriteLine($"{pgnGames.Count()} parsed.");

        }


        [Test, Explicit]
        public void can_play_lots_of_games()
        {
            var stream = File.OpenRead(@"C:\Src\Info\CSharpChess.UnitTests\bin\Debug\Modern.pgn");

            int count = 0;

            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {

                    Console.WriteLine($"Game index: {++count}");
                    if (count < 11500) continue;
                    PlayGame(game);

                    game = reader.ReadGame();

                    Console.Out.FlushAsync();
                }
            }
            Console.WriteLine(count);
            DumpMetrics();
        }

        [Test, Explicit]
        public void can_play_test_game()
        {
            PlayGame(PgnTestGames.TestGame);
            DumpMetrics();
        }

        [Test]
        public void can_play_pgn_with_piece_blocking_check()
        {
            var stream = File.OpenRead(@"C:\Src\Info\CSharpChess.UnitTests\bin\Debug\has-piece-blocking-check.pgn");
            
            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {
                    PlayGame(game);

                    game = reader.ReadGame();
                }
            }
        }

        [Test]
        public void can_play_pgn_with_pawn_promotion()
        {
            var stream = File.OpenRead(@"C:\Src\Info\CSharpChess.UnitTests\bin\Debug\has-black-pawn-to-queen-promotion.pgn");


            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {
                    var pgnGame = PgnGame.Parse(game).Single();
                    var board = new PgnGameResolver().Resolve(pgnGame);

                    var result = ChessGameResultToPgnResult(board.GameState);

                    if (IsInCheckmate(board.GameState))
                    {
                        Assert.That(pgnGame.Result, Is.EqualTo(result), pgnGame.ToString());
                    }

                    game = reader.ReadGame();
                }
            }
        }

        private IDictionary<string, long> _gameTimes = new ConcurrentDictionary<string, long>();

        private void PlayGame(string game)
        {

            var sw = new Stopwatch();
            sw.Start();

            var pgnGame = PgnGame.Parse(game).Single();
            Console.WriteLine($"Processing game: {pgnGame}");

            var board = new PgnGameResolver().Resolve(pgnGame);

            var result = ChessGameResultToPgnResult(board.GameState);

            if (result != ChessGameResult.Unknown) // Resignation & Draws can't be determined from the board state alone
            {
                Assert.That(pgnGame.Result, Is.EqualTo(result), pgnGame + "\n" + game);
            }

            var key = pgnGame.ToString() + Guid.NewGuid();
            _gameTimes.Add(key, sw.ElapsedMilliseconds);
            Console.WriteLine($"  Game took {_gameTimes[key]}ms");
        }

        private bool IsInCheckmate(GameState boardGameState)
        {
            return new[]
                {
                    GameState.CheckMateBlackWins,
                    GameState.CheckMateWhiteWins,
                }
                .Any(a => a == boardGameState);
        }

        private void DumpMetrics()
        {
            if (_gameTimes.Any())
            {
                var minMs = _gameTimes.Values.Min();
                var maxMs = _gameTimes.Values.Max();

                var fastest = _gameTimes.First(kvp => kvp.Value == minMs);
                var slowest = _gameTimes.First(kvp => kvp.Value == maxMs);
                var averageMs = _gameTimes.Average(kvp => kvp.Value);

                Console.WriteLine($"Count  : {_gameTimes.Count}");
                Console.WriteLine($"Slowest: {maxMs:######} : {slowest}");
                Console.WriteLine($"Fastest: {minMs:######} : {fastest}");
                Console.WriteLine($"Total  :  {TimeSpan.FromMilliseconds(_gameTimes.Sum(kvp => kvp.Value))}");
                Console.WriteLine($"Average: {averageMs:######}");
                _gameTimes.Clear();
            }
        }

        private static ChessGameResult ChessGameResultToPgnResult(GameState pgnGame)
        {
            switch (pgnGame)
            {
                case GameState.CheckMateBlackWins:
                    return ChessGameResult.BlackWins;
                case GameState.CheckMateWhiteWins:
                    return ChessGameResult.WhiteWins;
                case GameState.Stalemate:
                case GameState.Draw:
                    return ChessGameResult.Draw;
                case GameState.WhiteKingInCheck:
                case GameState.BlackKingInCheck:
                case GameState.WaitingForMove:
                    return ChessGameResult.Unknown;
                default:
                    throw new ArgumentOutOfRangeException(pgnGame.ToString());
            }
        }
    }
}
