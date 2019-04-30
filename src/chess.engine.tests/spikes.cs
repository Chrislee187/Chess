using System;
using System.Linq;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests
{
    [TestFixture]
    public class spikes
    {
        [Test]
        public void Should()
        {
            var engine = new ChessBoardEngine(new ChessBoardSetup(), new ChessMoveValidator(new MoveValidationFactory()), new ChessRefreshAllPaths());

            var startLocation = BoardLocation.At("B2");

            engine
                .AddEntity(ChessPieceEntityFactory.CreatePawn(Colours.White), startLocation)
                .AddEntity(ChessPieceEntityFactory.CreatePawn(Colours.Black), BoardLocation.At("D5"))
                .AddEntity(ChessPieceEntityFactory.CreatePawn(Colours.White), BoardLocation.At("C5"));

            var piece = engine.PieceAt("B2");

            Assert.That(piece.Entity.EntityType, Is.EqualTo(ChessPieceName.Pawn));

            var paths = piece.Paths;
            Assert.That(paths.Count(), Is.EqualTo(1));
            Assert.That(paths.SelectMany(m => m).Count(), Is.EqualTo(2));

            piece = engine.PieceAt("C5");
            Assert.That(piece.Entity.EntityType, Is.EqualTo(ChessPieceName.Pawn));
            paths = piece.Paths;
            Assert.That(paths.Count(), Is.EqualTo(2));
            Assert.That(paths.SelectMany(m => m).Count(), Is.EqualTo(2));
        }

        [Test]
        public void Spike_easy_board_builder_to_from_ChessGame()
        {
            var setup = new EasyBoardBuilder()
                    //                .Rank(8, "rnbqkbnr")
                    //                .File(ChessFile.A, "RP    pr")
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

            var game = new ChessGame(setup);

            var board = new EasyBoardBuilder().FromChessGame(game).ToString();
            Console.WriteLine(board);
        }
        [Test]
        public void Spike_easy_board_builder2()
        {
            var board = new EasyBoardBuilder()
                    //                .Rank(8, "rnbqkbnr")
                    //                .File(ChessFile.A, "RP    pr")
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
                    .ToString()
                ;


            Console.WriteLine(board);
        }
    }
}