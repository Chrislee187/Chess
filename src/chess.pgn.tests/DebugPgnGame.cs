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
            var pgnText = @"[Event ""BCF-ch""]
[Site ""Plymouth""]
[Date ""1989.??.??""]
[Round ""?""]
[White ""Adams, Michael""]
[Black ""Lane, Gary W""]
[Result ""1-0""]
[WhiteElo ""2505""]
[BlackElo ""2385""]
[ECO ""C65""]

1.e4 e5 2.Nf3 Nc6 3.Bb5 Bc5 4.O-O Nf6 5.c3 O-O 6.d4 Bb6 7.Bg5 h6 8.Bh4 d6
9.Re1 Bg4 10.Bxc6 bxc6 11.dxe5 dxe5 12.Qxd8 Raxd8 13.Nxe5 g5 14.Bg3 h5 15.h4 Nxe4
16.Rxe4 Rd1+ 17.Kh2 Bf5 18.Ra4 g4 19.Nc4 Re8 20.Nxb6 cxb6 21.Rxa7 c5 22.c4 Bxb1
23.b3 Ree1 24.Bf4 Kg7 25.Kg3 Rh1 26.Rb7 Rdg1 27.Be5+ Kg6 28.Kf4 Rxg2 29.Rxb6+ Kh7
30.Kg5 f6+ 31.Rxf6 Re1 32.Re6 Rxf2 33.Rxb1 Rxb1 34.Re8  1-0
";

            var pgnReader = PgnReader.FromString(pgnText);

            var game = pgnReader.ReadGame();

            var chessGame = ChessFactory.NewChessGame();

            try
            {
                PlayTurns(game, chessGame);
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