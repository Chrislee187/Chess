using System;
using board.engine;
using chess.engine.Extensions;
using chess.engine.Game;
using NUnit.Framework;

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
            // TODO: This has a race condition problem somewhere, only see it when
            // FeatureFlags.ParalleliseRefreshAllPaths = true;
            var pgnText = @"[Event ""Gibraltar Masters""]
 [Site ""Caleta ENG""]
 [Date ""2011.02.02""]
 [Round ""9.6""]
 [White ""Adams,Mi""]
 [Black ""Lemos,D""]
 [Result ""1/2-1/2""]
 [WhiteElo ""2723""]
 [BlackElo ""2553""]
 [ECO ""C07""]

 1.e4 e6 2.d4 d5 3.Nd2 c5 4.exd5 Qxd5 5.dxc5 Bxc5 6.Ngf3 Nf6 7.Bc4 Qc6 8.Qe2 O-O
 9.O-O Nbd7 10.Nb3 b6 11.Nxc5 Qxc5 12.b3 Bb7 13.Bb2 Rfd8 14.Rfd1 Nf8 15.Ne5 Ng6
 16.Nxg6 hxg6 17.Qe5 Qxe5 18.Bxe5 Nd5 19.Rd2 f6 20.Bd4 Kf7 21.a4 Nb4 22.Rad1 Bd5
 23.Bc3 Nxc2 24.Ba6 Rdc8 25.Bxc8 Rxc8 26.Rxc2 Bxb3 27.Rd7+ Ke8 28.Rcd2 Bd5
 29.Rxa7 Rxc3 30.h4 Kf8 31.Rd4 Kg8 32.Rb4 Kh7 33.f3 Rc2 34.Ra6 Kh6 35.Raxb6 g5
 36.Rd6 f5 37.Rd4 g6 38.hxg5+ Kxg5 39.a5 Ra2 40.a6 Kh5 41.Kf1 g5 42.Ke1 f4
 43.Rd2 Ra1+ 44.Rd1 Ra2 45.Rd2 Ra1+ 46.Kf2 Bc4 47.g4+ fxg3+ 48.Kxg3 Rxa6 49.Rd8 Kg6
 50.Rg8+ Kf6 51.Rd7 Bd5 52.Rf8+ Kg6 53.Rdf7 e5 54.Rf5 Re6 55.Kg4 Kg7 56.R8f7+ Kg6
 57.Rd7 Bc6 58.Rxg5+ Kf6 59.Rf5+ Kg6 60.Rg5+ Kf6 61.Rf5+ Kg6 62.Rd3 e4 63.Re3 Bd7
 64.Rg5+ Kf6 65.f4 Re5+ 66.Kh4 Re8 67.Kg3 Bf5 68.Ra3 Re6 69.Kf2 Rd6 70.Ra5 Bg6
 71.Rgb5 Bh7 72.f5 Kg5 73.Ke3 Rf6 74.Kxe4 Rxf5 75.Rxf5+ Kg4 76.Ke5 Bxf5 77.Ra4+ Kg5
 78.Ra1 Bc2 79.Re1 Bd3 80.Rg1+ Kh6 81.Kf6 Kh7 82.Rg3 Bb1 83.Rh3+ Kg8 84.Rh2 Bh7  1/2-1/2
";

            PlayAllGames(PgnReader.FromString(pgnText));
        }

        [Test]

        public void DebugBoardState()
        {
            var board = new ChessBoardBuilder()
                .Board("........" +
                       "........" +
                       "P..Rp..." +
                       "......pk" +
                       "..b..pP." +
                       ".....P.." +
                       "...R.K.." +
                       "r......."
                );
            var game = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.White);

            var msg = game.Move("fxg3+");

            Assert.That(msg, Is.EqualTo(""), msg);
            Console.WriteLine(game.ToTextBoard());
            Assert.Null(game.BoardState.GetItem("G1".ToBoardLocation()));

        }

        private static void PlayAllGames(PgnReader pgnReader)
        {
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
            chessGame.Move(gameTurnWhite.San);
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
            PlayAllGames(PgnReader.FromString(pgnText));

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