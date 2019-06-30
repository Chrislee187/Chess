using System;
using System.Linq;
using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.tests
{
    [TestFixture]
    public class spikes
    {

        private ChessBoardSetup _chessBoardSetup;

        // TODO: Smoke tests, for example: 
        // https://localhost:5001/api/ChessGame
        // should return newboard, inprogress, white turn, valid moves etc.
        // https://localhost:5001/api/ChessGame/....k%20%20%20%20ppppppp................................%20PPPPPPPRNBQKBNRW0000/a1a8
        // should return checkmate 
        // https://localhost:5001/api/ChessGame/asdasd
        // should return 404 invalid board 
        // https://localhost:5001/api/ChessGame/....k%20%20%20%20ppppppp................................%20PPPPPPPRNBQKBNRW0000/a1a2
        // should return 404 invalid move
        [SetUp]
        public void Setup()
        {
            _chessBoardSetup = new ChessBoardSetup(new ChessPieceEntityFactory());

        }
        [Test]
        public void Should()
        {
            var engine = ChessFactory.ChessBoardEngineProvider().Provide(_chessBoardSetup);

            var startLocation = "B2".ToBoardLocation();

            var piece = engine.BoardState.GetItem(startLocation);

            Assert.That(piece.Item.Piece, Is.EqualTo(ChessPieceName.Pawn));

            var paths = piece.Paths;
            Assert.That(paths.Count(), Is.EqualTo(1));
            Assert.That(paths.SelectMany(m => m).Count(), Is.EqualTo(2));
        }

        [Test]
        public void Spike_easy_board_builder_to_from_ChessGame()
        {
            var setup = new ChessBoardBuilder()
                    .Board("rnbqkbnr" +
                           "pppppppp" +
                           "        " +
                           "        " +
                           "        " +
                           "        " +
                           "PPPPPPPP" +
                           "RNBQKBNR")
                    .ToGameSetup()
                ;

            var game = ChessFactory.CustomChessGame(setup, Colours.White);

            var board = new ChessBoardBuilder().FromChessGame(game).ToTextBoard();
            Console.WriteLine(board);
        }

        [Test]
        public void Spike_easy_board_builder_to_from_ChessGame2()
        {
            var setup = new ChessBoardBuilder()
                    //                .X(8, "rnbqkbnr")
                    //                .Y(ChessFile.A, "RP    pr")
                    //                .At(ChessFile.D, 2, 'P')
                    //                .FromChessGame(new ChessGame())
                    .Board("rnbqkbnr" +
                           "pppppppp" +
                           "        " +
                           "        " +
                           "        " +
                           "        " +
                           "PPPPPPPP" +
                           "RNBQKBNR")
                    .ToGameSetup()
                ;


            var game = ChessFactory.CustomChessGame(setup, Colours.White);

            var board = new ChessBoardBuilder().FromChessGame(game).ToTextBoard();
            Console.WriteLine(board);
        }

        [Test]
        public void Spike_debugging_a_move_problem()
        {
            var board = new ChessBoardBuilder()
                    //                .X(8, "rnbqkbnr")
                    //                .Y(ChessFile.A, "RP    pr")
                    //                .At(ChessFile.D, 2, 'P')
                    //                .FromChessGame(new ChessGame())
                    .Board("r.bq.rk." +
                           "...nbppp" +
                           "p.pp.n.." +
                           ".p..p..." +
                           "..PPP..." +
                           ".B...N.P" +
                           "PP...PP." +
                           "RNBQR.K.")
                ;

            var game = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.White);
//            var game = new ChessGame(NullLogger<ChessGame>.Instance, _chessBoardEngineProvider, board.ToGameSetup(), Colours.White);

            // TODO: Fix this bug, pawn can't take pawn, something to do with
            // enpassant i guess based on pawn positions

            var msg = game.Move("c4b5");

            Assert.IsEmpty(msg, msg);
        }
    }
}

