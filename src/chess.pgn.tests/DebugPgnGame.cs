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
            var pgnText = @"[Event ""Tilburg rapid""]
 [Site ""Tilburg""]
 [Date ""1992.??.??""]
 [Round ""3""]
 [White ""Beliavsky, Alexander G""]
 [Black ""Adams, Michael""]
 [Result ""0-1""]
 [WhiteElo ""2595""]
 [BlackElo ""2610""]
 [ECO ""B07""]

 1.d4 d6 2.e4 Nf6 3.Nc3 e5 4.dxe5 dxe5 5.Qxd8+ Kxd8 6.Bg5 Be6 7.O-O-O+ Nd7
 8.f4 exf4 9.Nf3 h6 10.Bxf4 c6 11.Bd3 Bc5 12.h3 Ke7 13.Na4 g5 14.Bh2 Be3+
 15.Kb1 Nh5 16.Rhe1 Bf4 17.Bg1 Bg3 18.Rf1 Bd6 19.e5 Bc7 20.Nc5 Nxc5 21.Bxc5+ Ke8
 22.Nd4 Ng7 23.Rde1 Bb6 24.Bxb6 axb6 25.g4 Bxa2+ 26.Kc1 Bd5 27.Kd2 b5 28.Rf2 Rd8
 29.Nf5 Nxf5 30.gxf5 Bc4 31.f6 Bxd3 32.cxd3 Kd7 33.h4 Ke6 34.Rg1 gxh4 35.Rf4 Rd5
 36.d4 Rhd8 37.Kc3 b4+  0-1
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