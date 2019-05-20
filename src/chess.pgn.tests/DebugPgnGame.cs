using System;
using board.engine;
using board.engine.Movement;
using chess.engine.Extensions;
using chess.engine.Game;
using NUnit.Framework;

namespace chess.pgn.tests
{
    [TestFixture]
    [Explicit("Test for use in debugging individual game problems")]
    public class DebugPgnGame
    {

        [Test]
        public void DebugPgnGameTest()
        {
            var pgnText = @"[Event ""Hastings8990""]
 [Site ""Hastings""]
 [Date ""1989.??.??""]
 [Round ""?""]
 [White ""Spraggett, Kevin""]
 [Black ""Adams, Michael""]
 [Result ""1-0""]
 [WhiteElo ""2585""]
 [BlackElo ""2505""]
 [ECO ""A25""]

 1.g3 Nf6 2.Bg2 e5 3.c4 Nc6 4.Nc3 Bb4 5.Nd5 O-O 6.a3 Bd6 7.Nc3 Re8 8.d3 Bf8
 9.e3 Ne7 10.e4 c6 11.Bg5 Ng6 12.Nge2 h6 13.Bd2 Ne7 14.f4 exf4 15.gxf4 d5
 16.e5 Ng4 17.h3 d4 18.hxg4 dxc3 19.Bxc3 Bxg4 20.d4 Ng6 21.O-O Nh4 22.Be4 Nf5
 23.Bxf5 Bxf5 24.Ng3 Be6 25.b3 f5 26.Qd3 Qd7 27.a4 Qf7 28.Rad1 Rad8 29.Rd2 Rd7
 30.Qf3 Red8 31.Rfd1 Kh7 32.Nf1 g5 33.Ne3 g4 34.Qh1 Rc7 35.Kf1 b6 36.Bb2 Qg6
 37.d5 cxd5 38.Nxd5 Bxd5 39.cxd5 Bb4 40.d6 Rg7 41.Rc2 Bc5 42.Rd3 a5 43.Bd4 h5
 44.Bxc5 bxc5 45.Rh2 Kh6 46.e6 c4 47.e7 Rb8 48.d7 cxd3 49.d8=Q  1-0
";

            var pgnReader = PgnReader.FromString(pgnText);

            var game = pgnReader.ReadGame();

            var chessGame = ChessFactory.NewChessGame();

            try
            {
                PlayTurns(game, chessGame);
                Assert.Pass();
            }
            catch
            {
                Console.WriteLine($"Full PGN Text:\n{pgnReader.LastGameText}");
                throw;
            }

        }

        [Test]
        public void DebugBoardState()
        {
            //            var board = new ChessBoardBuilder()
            //                .Board(".r......" +
            //                       "...PP.r." +
            //                       "......qk" +
            //                       "p....p.p" +
            //                       "P....Pp." +
            //                       ".P.p...." +
            //                       ".......R" +
            //                       ".....K.Q"
            //                );
            var board = new ChessBoardBuilder()
                .Board(".r......" +
                       "...PP.r." +
                       ".......k" +
                       "........" +
                       "........" +
                       "........" +
                       "........" +
                       ".....K.Q"
                );
            var game = ChessFactory.CustomChessGame(board.ToGameSetup());

            Console.WriteLine(game.BoardState.GetItem("d7".ToBoardLocation()).Paths);
            Console.WriteLine(game.BoardState.GetItem("e7".ToBoardLocation()).Paths);

            game.Move("d8=Q");


        }
        private static void PlayTurns(PgnGame game, ChessGame chessGame)
        {
            var previousBoard = chessGame.ToText();
            foreach (var gameTurn in game.Turns)
            {
                if (gameTurn.White != null)
                {
                    Console.WriteLine($"Playing white move {gameTurn.Turn}: {gameTurn.White.San}");
                    chessGame.Move(gameTurn.White.San);
                    var board = chessGame.ToText();
                    Console.WriteLine(board);
                    Assert.That(previousBoard, Is.Not.EqualTo(board));
                    previousBoard = board;
                }


                if (gameTurn.Black != null)
                {
                    Console.WriteLine($"Playing black move {gameTurn.Turn}: {gameTurn.Black.San}");
                    chessGame.Move(gameTurn.Black.San);
                    var board = chessGame.ToText();
                    Console.WriteLine(board);
                    Assert.That(previousBoard, Is.Not.EqualTo(board));
                    previousBoard = board;
                }
            }

        }

        [Test]
        public void Regression_KeepAndConvertToUnitTest()
        {
            // TODO: 
            // Convert to a unit test, the problem was that the ContainsMoveTo() wasn't 
            // matching on all correct movetypes

            var board = new ChessBoardBuilder()
                .Board("........" +
                       "R....pk." +
                       ".p......" +
                       "..p....p" +
                       "..P..BpP" +
                       ".P......" +
                       "P....PPK" +
                       "Rb.rr..."
                );
            var game = ChessFactory.CustomChessGame(board.ToGameSetup());

            var king = game.BoardState.GetItem("h2".ToBoardLocation());

            Assert.True(king.Paths.ContainsMoveTo("G3".ToBoardLocation()));
            Assert.True(!king.Paths.ContainsMoveTo("H3".ToBoardLocation()));
        }
    }
}