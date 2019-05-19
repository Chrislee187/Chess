using System;
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
            var pgnText = @"[Event ""London NWYM""]
 [Site ""London""]
 [Date ""1987.??.??""]
 [Round ""?""]
 [White ""Vlahos, Kiriakos""]
 [Black ""Adams, Michael""]
 [Result ""0-1""]
 [WhiteElo ""2275""]
 [BlackElo ""2360""]
 [ECO ""A28""]

 1.c4 e5 2.Nc3 Nf6 3.Nf3 Nc6 4.e3 Bb4 5.Qc2 O-O 6.Nd5 Re8 7.Qf5 d6 8.Nxf6+ gxf6
 9.Qh5 d5 10.Bd3 e4 11.cxd5 exd3 12.dxc6 bxc6 13.b3 Bf8 14.Bb2 Re4 15.h3 Rb8
 16.Nd4 Qd5 17.Qxd5 cxd5 18.Nf3 Re6 19.O-O Ba6 20.Rfc1 Rb7 21.Nd4 Reb6 22.Nf3 Bb5
 23.Bd4 Ba3 24.Bxb6 axb6 25.Nd4 Bxc1 26.Rxc1 Bd7 27.Rc3 c5 28.Nf3 Bb5 29.a4 Ba6
 30.b4 Bc4 31.bxc5 bxc5 32.Kh2 Rb2 33.Kg3 Rc2 34.Kf4 Kf8 35.g4 h6 36.h4 Ke7
 37.Kf5 Ba6 38.g5 fxg5 39.hxg5 h5 40.Rb3 c4 41.Ra3 d4 42.exd4  0-1
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
            var board = new ChessBoardBuilder()
                .Board("........" +
                       "....kp.." +
                       "b....p.p" +
                       "..pp.K.." +
                       "P.....PP" +
                       "..RpPN.." +
                       "..rP.P.." +
                       "........"
                );
            var game = ChessFactory.CustomChessGame(board.ToGameSetup());

            var piece = game.BoardState.GetItem("g4".ToBoardLocation());

            Assert.True(piece.Paths.ContainsMoveTo("g5".ToBoardLocation()));
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