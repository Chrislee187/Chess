using System;
using System.Linq;
using board.engine;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.pgn.Parsing;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace chess.pgn.tests
{
    [TestFixture]
//    [Explicit("Test for use in debugging individual game problems")]
    public class DebugPgnGame
    {

        [Test]
//        [Repeat(10)]
        public void DebugPgnGameTest()
        {
            // FeatureFlags.ParalleliseRefreshAllPaths = true;
            var pgnText = @"[Event ""RUS-ch02""]
[Site ""Moscow""]
[Date ""1901.??.??""]
[Round ""10""]
[White ""Grigoriev, B.""]
[Black ""Yankovich, Boris""]
[Result ""1-0""]
[WhiteElo """"]
[BlackElo """"]
[ECO ""A42""]
1.e4 d6 2.d4 g6 3.c4 Bg7 4.Nc3 Nc6 5.Be3 e5 6.d5 Nce7 7.c5 Nh6 8.f3 f5 9.cxd6 cxd6
10.Bb5+ Kf8 11.Qa4 f4 12.Bf2 Bf6 13.Nge2 Kg7 14.Rc1 a6 15.O-O g5 16.Rc2 Rb8
17.Bd3 Nf7 18.Rfc1 Qd7 19.Bb6 Qxa4 20.Nxa4 Bd7 21.Nec3 Rbc8 22.Ba5 Bxa4 23.Nxa4 Rxc2
24.Rxc2 Rc8 25.Nb6 Rxc2 26.Bxc2 h5 27.Ba4 g4 28.Bd7 gxf3 29.gxf3 Ng5 30.Kf2 Kf8
31.Nc4 Ng6 32.Bc8 Be7 33.Bxb7 Nh4 34.Nd2 Nh3+ 35.Kf1  1-0";

            PlayGame(PgnGame.ReadAllGamesFromString(pgnText).First());
        }

        [Test]

        public void DebugBoardState()
        {
            var board = new ChessBoardBuilder()
                .Board("........" +
                       "........" +
                       "P..Rp..." +
                       "......pk" +
                       "..b..pE." +
                       ".....P.." +
                       "...R.K.." +
                       "r......."
                );
            var game = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.Black);

            var msg = game.Move("fxg3+");

            StringAssert.DoesNotContain(msg, "ERROR");
            Console.WriteLine(game.ToTextBoard());
            Assert.Null(game.BoardState.GetItem("G1".ToBoardLocation()));

        }

        private static void PlayGame(PgnGame game)
        {

            var chessGame = ChessFactory.NewChessGame();

            try
            {
                PlayTurns(game, chessGame);
            }
            catch
            {
                Console.WriteLine($"Full PGN Text:\n{game.PgnText}");
                throw;
            }
            Assert.Pass();
        }
        private static void PlayTurns(PgnGame game, ChessGame chessGame)
        {
            var previousBoard = chessGame.ToTextBoard();
            foreach (var gameTurn in game.Turns)
            {
                if (gameTurn.White != null)
                {
                    Console.WriteLine($"Playing white move {gameTurn.Turn}: {gameTurn.White.San}");
                    var board = AssertMoveWasExecuted(chessGame, previousBoard, gameTurn.White);
                    previousBoard = board;
                }


                if (gameTurn.Black != null)
                {
                    Console.WriteLine($"Playing black move {gameTurn.Turn}: {gameTurn.Black.San}");
                    var board = AssertMoveWasExecuted(chessGame, previousBoard, gameTurn.Black);
                    previousBoard = board;
                }
            }

        }

        private static string AssertMoveWasExecuted(ChessGame chessGame, string previousBoard, PgnMove gameTurnWhite)
        {
            var msg = chessGame.Move(gameTurnWhite.San);
            var board = chessGame.ToTextBoard();
            Console.WriteLine(board);
            Assert.That(!previousBoard.Equals(board), $"'{gameTurnWhite.San}' not executed!");
            return board;
        }

        [Test]
        public void Weird_castling_bug_game()
        {
            // Strange bug around castling, for some reason the validated paths don't always
            // know that the bishop attack path (that blocks castling) gets blocked by the pawn move
            // Doesn't seem to show itself her but consistently appears (when not running with paralleise options)
            // as part of the game it's from
            var pgnText = @"[Event ""PCA/Intel-GP""]
 [Site ""New York""]
 [Date ""1994.??.??""]
 [Round ""1""]
 [White ""Adams, Michael""]
 [Black ""Malaniuk, Vladimir P""]
 [Result ""1-0""]
 [WhiteElo ""2640""]
 [BlackElo ""2610""]
 [ECO ""C48""]

 1.e4 e5 2.Nf3 Nc6 3.Nc3 Nf6 4.Bb5 Nd4 5.Ba4 c6 6.Nxe5 d5 7.d3 Bd6 8.f4 Bc5
 9.exd5 b5 10.Bb3 cxd5 11.Ne2 Nxb3 12.axb3 d4 13.O-O O-O 14.Ng3 Bb7 15.Bd2 Re8
 16.Re1 Bb6 17.Qe2 Rc8 18.Rac1 g6 19.Qf2 Nd5 20.f5 Qc7 21.Nf3 Rxe1+ 22.Qxe1 Ne3
 23.Qe2 gxf5 24.Bxe3 dxe3 25.Nxf5 Kh8 26.Kh1 Qf4 27.Ng3 Rc6 28.Rf1 Rh6 29.Kg1 Rg6
 30.c3 Bc7 31.d4 Qg4 32.Qxe3 Bxg3 33.hxg3 Qxg3 34.Qe5+ Qxe5 35.dxe5 a5 36.Kf2 a4
 37.b4 Re6 38.Re1 Bd5 39.Nd4 Rb6 40.Rd1 Bc4 41.Nf5 h5 42.Rd6 Rb7 43.Rh6+ Kg8
 44.Rxh5 a3 45.bxa3 Ra7 46.Nd6 Rxa3 47.Nxc4 bxc4 48.Rh3 Kf8 49.Ke2 Ke7 50.Kd2 Ke6
 51.Rh4 Ra2+ 52.Ke3  1-0
";
            PlayGame(PgnGame.ReadAllGamesFromString(pgnText).First());

        }

        [Test]
        public void Convert_to_test_for_path_refresh_ordering()
        {
            var board = new ChessBoardBuilder()
                .Board("r.bqk..r" +
                       "p....ppp" +
                       ".....n.." +
                       ".pbpN..." +
                       ".....P.." +
                       ".P.P...." +
                       ".PP.N.PP" +
                       "R.BQK..R"
                );
            var game = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.Black);

            var whiteKing = game.BoardState.GetItem("E1".ToBoardLocation());

            Assert.False(whiteKing.Paths.ContainsMoveTo("g1".ToBoardLocation()));
            Console.WriteLine(whiteKing.Paths);


            game.Move("d4");
            var king = game.BoardState.GetItem("e1".ToBoardLocation());

            Assert.That(king.Paths.ContainsMoveTo("g1".ToBoardLocation()));

            var moveResponse = game.Move("O-O");
            Assert.IsEmpty(moveResponse);
            Assert.Null(game.BoardState.GetItem("e1".ToBoardLocation()));
            king = game.BoardState.GetItem("g1".ToBoardLocation());
            Assert.NotNull(king);
            Assert.That(king.Location, Is.EqualTo("g1".ToBoardLocation()));

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