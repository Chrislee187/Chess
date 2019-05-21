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
            FeatureFlags.ParalleliseRefreshAllPaths = true;
            var pgnText = @"[Event ""Yalta op""]
 [Site ""Yalta""]
 [Date ""1996.??.??""]
 [Round ""5""]
 [White ""Bezman, Vadim""]
 [Black ""Chehlov, Aleksander""]
 [Result ""1-0""]
 [WhiteElo ""2365""]
 [BlackElo ""2270""]
 [ECO ""E84""]

 1.c4 g6 2.e4 Bg7 3.d4 d6 4.Nc3 Nf6 5.f3 O-O 6.Be3 Nc6 7.Nge2 a6 8.Qd2 Rb8
 9.h4 h5 10.Bh6 e5 11.Bxg7 Kxg7 12.O-O-O b5 13.cxb5 axb5 14.Kb1 b4 15.Nd5 b3
 16.a3 Nxd5 17.exd5 Bf5+ 18.Ka1 Nb4 19.Ng3 Nc2+ 20.Kb1 Nxa3+ 21.Kc1 Bc2 22.bxa3 Bxd1
 23.Qxd1 b2+ 24.Kb1 Rb6 25.Bd3 Qa8 26.Qd2 f6 27.Qc3 Rf7 28.dxe5 dxe5 29.Ne4 c5
 30.Nd2 Ra7 31.Nc4 Rba6 32.Bc2 Qxd5 33.Rd1 Qe6 34.Qd3 f5 35.Qc3 Kh7 36.Re1 Qe8
 37.g3 Re6 38.Kxb2 e4 39.fxe4 fxe4 40.Re3 Rd7 41.Nd2 Rd4 42.Nb3 Rd5 43.Qc4 Rde5
 44.Ka2 Qf8 45.Nd2 Qf2 46.Qe2 Qxe2 47.Rxe2 Kg7 48.Nxe4 g5 49.hxg5 Rxg5 50.Kb3 Rge5
 51.Bd3 Re7 52.a4 Rb7+ 53.Bb5 Rbe7 54.Nc3 Rxe2 55.Nxe2 Kf6 56.a5 Re5 57.Kb2 Kg5
 58.a6 Re7 59.Kc3 Kg4 60.Kd3  1-0
";

            PlayAllGames(PgnReader.FromString(pgnText));
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

        [Test]

        public void DebugBoardState()
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
        private static void PlayTurns(PgnGame game, ChessGame chessGame)
        {
            var previousBoard = chessGame.ToText();
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
            var board = chessGame.ToText();
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