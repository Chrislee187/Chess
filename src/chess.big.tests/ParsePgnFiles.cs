using System;
using System.IO;
using System.Runtime.InteropServices;
using chess.pgn;
using NUnit.Framework;

namespace chess.big.tests
{
    [TestFixture, Explicit]
    public class ParsePgnFiles
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(@".\PGNFiles")]
        [TestCase(@"D:\Src\PGNArchive\PGN")]
        public void ParseAllFiles(string path)
        {
            var pgnfiles = Directory.GetFiles(path, "*.pgn", SearchOption.AllDirectories);
            var fileCount = 0;
            var gamesCount = 0;
            var fileGamesCount = 0;
            foreach (var file in pgnfiles)
            {
                fileCount++;
                if (file.Contains("FAILED")) continue;

                try
                {
                    var pgnReader = PgnReader.FromFile(file);

                    var game = pgnReader.ReadGame();
                    while (game != null)
                    {
                        fileGamesCount++;
                        // DumpGameInfo(game);
                        game = pgnReader.ReadGame();
                    }
                    gamesCount += fileGamesCount;
                }
                catch 
                {
                    Console.WriteLine($"Failed: {file}\nGame Idx: {fileCount}/{fileGamesCount}");
                    throw;
                }

                fileGamesCount = 0;
            }

            Console.WriteLine($"Files #: {fileCount}, Total Games #: {gamesCount}");
            Assert.That(fileCount, Is.GreaterThan(0), "No files processed");
            Assert.That(gamesCount, Is.GreaterThan(0), "No games processed");
        }

        [TestCase(@"D:\Src\PGNArchive\PGN\Adams\Adams.pgn")]
//        [TestCase(@"D:\Src\PGNArchive\PGN\Nielsen\Nielsen.pgn")]
        public void Parse_single_file(string filename)
        {
            ParseSingleFile(filename);
        }

        private static void ParseSingleFile(string file)
        {
            PgnGame game = null;
            var count = 1;
            try
            {
                var pgnReader = PgnReader.FromFile(file);

                game = pgnReader.ReadGame();

                while (game != null)
                {
//                    DumpGameInfo(game);
                    game = pgnReader.ReadGame();
                    count++;
                }
            }
            catch 
            {
                Console.WriteLine($"Failed: #{count}, {file}, {game?.ToString() ?? ""}");
                if(game != null) DumpGameInfo(game);
                throw;
            }

            Console.WriteLine($"{count} games parsed from {file}");
        }

        private static void DumpGameInfo(PgnGame game)
        {
            Console.WriteLine($"Game Parsed: {game}");
        }
    }
}