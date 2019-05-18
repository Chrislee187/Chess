using System;
using System.Diagnostics;
using System.IO;
using chess.engine.Game;
using chess.pgn;
using NUnit.Framework;

namespace chess.big.tests
{
    [TestFixture, Explicit]
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

                PlaySingleFile(file);
            }

            Console.WriteLine($"Files #: {fileCount}, Total Games #: {gamesCount}");
            Assert.That(fileCount, Is.GreaterThan(0), "No files processed");
            Assert.That(gamesCount, Is.GreaterThan(0), "No games processed");
        }

        [TestCase(@"D:\Src\PGNArchive\PGN\Adams\Adams.pgn")]
//        [TestCase(@"D:\Src\PGNArchive\PGN\Nielsen\Nielsen.pgn")]
        public void Play_single_file(string filename)
        {
            PlaySingleFile(filename);
        }

        private void PlaySingleFile(string filename)
        {
            PgnGame game = null;
            ChessGame chessGame = null;
            PgnTurn lastTurn = null;
            var gameIdx = 0;
            var pgnReader = PgnReader.FromFile(filename);
            try
            {
                game = pgnReader.ReadGame();
                while (game != null)
                {
                    gameIdx++;

                    chessGame = ChessFactory.NewChessGame();
                    Debug.WriteLine($"{gameIdx}, ");
                    PlayTurns(game, chessGame);

                    game = pgnReader.ReadGame();
                }
            }
            catch 
            {
                Console.WriteLine($"File: {filename}");
                Console.WriteLine($"Game: #{gameIdx} / {game?.ToString() ?? ""}");
                Console.WriteLine($"Board:\n{chessGame.ToText()}");
                Console.WriteLine($"Full PGN Text:\n{pgnReader.LastGameText}");
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