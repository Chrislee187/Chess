using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using chess.pgn;
using chess.pgn.Parsing;
using NUnit.Framework;

namespace chess.big.tests
{
    [TestFixture]
    public class ParsePgnFiles
    {
        [SetUp]
        public void Setup()
        {
        }

//        [TestCase(@".\PGNFiles")]
//        [TestCase(@"D:\Src\PGNArchive\PGN")]
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
                    var games = PgnGame.ReadAllGamesFromString(File.ReadAllText(file));

                        // DumpGameInfo(game);
                    gamesCount += games.Count();
                }
                catch 
                {
                    Console.WriteLine($"Failed: {file}\nGame Idx: {fileCount}/{fileGamesCount}");
                    throw;
                }

                fileGamesCount = 0;
            }

            TestContext.Progress.WriteLine($"Files #: {fileCount}, Total Games #: {gamesCount}");
            Assert.That(fileCount, Is.GreaterThan(0), "No files processed");
            Assert.That(gamesCount, Is.GreaterThan(0), "No games processed");
        }

//        [TestCase(@"D:\Src\PGNArchive\PGN\Adams\Adams.pgn")]
        [Explicit]
        [TestCase(@"D:\Src\PGNArchive\PGN\Nielsen\Nielsen.pgn")]
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
                var games = PgnGame.ReadAllGamesFromString(File.ReadAllText(file));
                    count = games.Count();
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