using System;
using chess.engine.Game;
using chess.pgn;
using NUnit.Framework;

namespace chess.big.tests
{
    [TestFixture]
    [Explicit("Test for use in debugging individual game problems")]
    public class DebugPgnGame
    {

        [Test]
        public void DebugPgnGameTest()
        {
            var pgnText = @"[Event ""Bad Lauterberg""]
[Site ""Bad Lauterberg""]
[Date ""1991.??.??""]
[Round ""3""]
[White ""Mohr, Stefan""]
[Black ""Nielsen, Peter Heine""]
[Result ""1/2-1/2""]
[WhiteElo ""2465""]
[BlackElo ""2445""]
[ECO ""A53""]

1.d4 Nf6 2.c4 d6 3.Nc3 Nbd7 4.e4 e5 5.d5 Nc5 6.Qc2 a5 7.Nf3 c6 8.Be3 Qc7
9.Be2 Bd7 10.Nd2 Be7 11.O-O O-O 12.Rfc1 h6 13.b3 Nh7 14.a3 Bg5 15.Bxg5 Nxg5
16.h4 Nh7 17.Rab1 Na6 18.Qd1 c5 19.Bg4 Nf6 20.Bxd7 Qxd7 21.Qe2 Rfc8 22.h5 g6
23.hxg6 fxg6 24.g3 Kg7 25.Kg2 Rf8 26.Rh1 Rf7 27.Nd1 Raf8 28.Ne3 Nc7 29.a4 Nce8
30.Rh4 Nh7 31.Rf1 Nef6 32.Rfh1 Ng8 33.Ng4 h5 34.f3 Ng5 35.Nh2 Qe7 36.Qe3 Nh6
37.Qc3 Nh7 38.Qxa5 Nf6 39.Nhf1 g5 40.R4h2 g4 41.Ne3 gxf3+ 42.Kf1 Nfg4 43.Nxg4 Nxg4
44.Rxh5 Ne3+ 45.Kf2 Ng4+ 46.Kf1 Ne3+ 47.Kf2 Ng4+ 48.Ke1  1/2-1/2";

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
    }
}