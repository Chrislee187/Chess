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
            var pgnText = @"[Event ""CSR-ch""]
 [Site ""Prague""]
 [Date ""1955.??.??""]
 [Round ""17""]
 [White ""Rejfir, Josef""]
 [Black ""Alster, Ladislav""]
 [Result ""1-0""]
 [WhiteElo """"]
 [BlackElo """"]
 [ECO ""E80""]

 1.c4 g6 2.d4 Bg7 3.Nc3 d6 { this is a comment } 4.e4 Nf6 5.f3 e5 6.dxe5 dxe5 7.Qxd8+ Kxd8 8.Bg5 c6
 9.Nge2 Nd7 10.Rd1 Kc7 11.Nc1 Nc5 12.Be3 Nfd7 13.b4 Ne6 14.Nb3 Bf8 15.c5 a5
 16.Nxa5 Nexc5 17.a3 Ne6 18.Kf2 Be7 19.Rc1 Bg5 20.Bxg5 Nxg5 21.Bc4 Nb6 22.h4 Ne6
 23.Nb5+ Kb8 24.Nd6 Nxc4 25.Naxc4 f6 26.Rc3 Ra6 27.Rhc1 Rd8 28.Nxc8 Kxc8 29.a4 Kd7
 30.Rd1+ Ke7 31.Rxd8 Kxd8 32.a5 Ke7 33.Nb6 c5 34.Nd5+ Kf7 35.bxc5 Rc6 36.Ke3 f5
 37.g3 Nxc5 38.Nb4 f4+ 39.gxf4 exf4+ 40.Ke2 Rc7 41.Nd3 Ne6 42.Rxc7+ Nxc7 43.Nxf4 Kf6
 44.Nd3 Ne6 45.Ke3 Ke7 46.Nf4 Nc5 47.Nd5+ Kf7 48.Nb6 Ke6 49.Nc4 h6 50.f4 Nb3
 51.h5 gxh5 52.f5+ Ke7 53.e5 h4 54.Kf4 h5 55.f6+ Ke6 56.Nd6 Nxa5 57.f7 Ke7
 58.e6 Nc6 59.Nf5+ Kf8 60.Nxh4 Ne7 61.Ke5 Kg7 62.Kd6 Kf8 63.Kc5 Kg7 64.Kc4 Kf8
 65.Kd4 Kg7 66.Kc5 Kf8 67.Kd6 b6 68.Ke5 Kg7 69.Kd4 Nc6+ 70.Kd5 Ne7+ 71.Ke5 b5
 72.Kd6 Kf8 73.Kc5 Kg7 74.Kxb5 Nd5 75.Kc6  1-0
";

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