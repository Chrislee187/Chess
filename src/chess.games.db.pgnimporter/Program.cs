using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using chess.games.db.api;
using chess.games.db.Entities;
using PgnReader;

namespace chess.games.db.pgnimporter
{
    class Program
    {
        private static ChessGamesDbContext _dbContext = new ChessGamesDbContext();

        static void Main(string[] args)
        {
            var repo = new GamesRepository(_dbContext);

            // TODO: Better handling of paths, sort YACLAP out and add some proper CLA's
            var path = args.Any() ? args[0] : @"D:\src\PGNArchive\PGN";
            var recurse = true;

            string[] pgnfiles;

            if (Directory.Exists(path))
            {
                pgnfiles = Directory.GetFiles(path, "*.pgn", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).OrderBy(o => o).ToArray();
            }
            else if (File.Exists(path))
            {
                pgnfiles = new[] {path};
            }
            else
            {
                throw new ArgumentException("No filename or path specified");
            }

            //            var gamesCount = 0;
            //            var sw = Stopwatch.StartNew();
            //            var fileTimes = new List<TimeSpan>();
            //            var allGameTimes = new List<TimeSpan>();
            Console.WriteLine($"Beginning import of {pgnfiles.Length} files at: {DateTime.Now}");
            var fileCount = 0;
            pgnfiles.ToList().ForEach(file =>
            {
                fileCount++;

                var sw = Stopwatch.StartNew();
                Console.WriteLine($"File #{fileCount}/{pgnfiles.Length} : {file}");

                var pgnGames = PgnGame.ReadAllGamesFromFile(file);
                var gameCount = 0;
                foreach (var pgnGame in pgnGames)
                {
                    gameCount++;
                    var gameResult = repo.GetOrCreate(pgnGame).Result;
                    Console.Write(DotProgress(gameCount));
                }
                sw.Stop();
                Console.WriteLine();
                Console.WriteLine($"File complete, {pgnGames.Count()} games added to DB, DB Total Games: {repo.Select().Count()}");
                Console.WriteLine($"  time taken: {sw.Elapsed}, avg. time per game: {new TimeSpan(Convert.ToInt64(sw.Elapsed.Ticks / pgnGames.Count()))}");

            });
        }
        private static string DotProgress(int count)
        {
            return count % 1000 == 0 ? "M"
                : count % 500 == 0 ? "D"
                : count % 100 == 0 ? "C"
                : ".";
        }
    }
}
