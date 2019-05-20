using System;
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