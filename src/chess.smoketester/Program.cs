using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using chess.engine.Game;
using chess.pgn;

namespace chess.smoketester
{
    class Program
    {

        static void Main(string[] args)
        {
            var path = args.Any() ? args[0] : @"D:\src\PGNArchive\PGN";
            var recurse = true;
            var pgnfiles = Directory.GetFiles(path, "*.pgn", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).OrderBy(o => o).ToArray();
            var gamesCount = 0;
            var sw = Stopwatch.StartNew();
            var fileTimes = new List<TimeSpan>();
            var allGameTimes = new List<TimeSpan>();
            Console.WriteLine($"Beginning run at: {DateTime.Now}");
            // NOTE: Parallelising at this levels slows batch processing down considerably
            pgnfiles.ToList().ForEach(file =>
            {
                var (gameCount, fileTime, gameTimes) = PlayFile(file);
                gamesCount += gameCount;
                fileTimes.Add(fileTime);
                allGameTimes.AddRange(gameTimes);
            });
            sw.Stop();
            Console.WriteLine($"Files #: {pgnfiles.Length}, Total Games #: {gamesCount}");
            Console.WriteLine($"Total time: {sw.Elapsed}");
            Console.WriteLine($"  Average file time: {new TimeSpan(sw.ElapsedTicks / pgnfiles.Length)}");
            Console.WriteLine($"  Average game time: {new TimeSpan(sw.ElapsedTicks / gamesCount)}");
            Console.WriteLine($"  Average all game time: {new TimeSpan(Convert.ToInt64(allGameTimes.Average(ts => ts.Ticks)))}");
            Console.WriteLine($"Finished run at: {DateTime.Now}");
        }

        private static (int, TimeSpan, IEnumerable<TimeSpan>) PlayFile(string file)
        {
            var fileTimes = new List<TimeSpan>();
            Console.WriteLine($"*** Starting file: {file} at: {DateTime.Now}");
            var fileSw = Stopwatch.StartNew();
            var games = PgnGame.ReadAllGamesFromFile(file);

            var gameTimes = PlayAllGames(games);

            fileSw.Stop();
            fileTimes.Add(fileSw.Elapsed);

            Console.WriteLine($"*** Finished file: {file} at {DateTime.Now}, duration: {fileSw.Elapsed}");
            return (games.Count(), fileSw.Elapsed, gameTimes);
        }

        private static IEnumerable<TimeSpan> PlayAllGames(IEnumerable<PgnGame> games)
        {
            var loggerType = ChessFactory.LoggerType.Null;
            ChessGame chessGame = null;
            var gameIdx = 0;
            string lastPgnText = null;
            var timings = new List<TimeSpan>();
            try
            {

                chessGame = ChessFactory.NewChessGame(loggerType);
                games.ToList().ForEach(game =>
                {

                    lastPgnText = game.PgnText;
                    gameIdx++;
                    Console.Write(gameIdx % 1000 == 0 ? "M"
                        : gameIdx % 500 == 0 ? "D"
                        : gameIdx % 100 == 0 ? "C"
                        : ".");
                    chessGame = ChessFactory.NewChessGame(loggerType);
                    var sw = Stopwatch.StartNew();

                    PlayTurns(game, chessGame);

                    timings.Add(sw.Elapsed);
                });
                Console.WriteLine();
                Console.WriteLine($"Average playtime ({new TimeSpan(Convert.ToInt64(timings.Average(ts => ts.Ticks)))})");
            }
            catch
            {
                Console.WriteLine($"FAILED");
                Console.WriteLine($"Game: #{gameIdx} / {games.Count()}");
                Console.WriteLine($"Board:\n{chessGame.ToTextBoard()}");
                Console.WriteLine($"Full PGN Text:\n{lastPgnText}");
                throw;
            }

            return timings;
        }

        private static IEnumerable<TimeSpan> PlayTurns(PgnGame game, ChessGame chessGame)
        {
            PgnTurn lastTurn = null;
            var timings = new List<TimeSpan>();
            try
            {
                foreach (var gameTurn in game.Turns)
                {
                    var sw = Stopwatch.StartNew();
                    lastTurn = gameTurn;
                    if (gameTurn.White != null)
                        chessGame.Move(gameTurn.White.San);

                    if (gameTurn.Black != null)
                        chessGame.Move(gameTurn.Black.San);

                    timings.Add(sw.Elapsed);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Problem with: {lastTurn}", e);
            }

            return timings;
        }
    }
}
