using System;
using System.Linq;
using chess.engine.Actions;
using chess.engine.Board;
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
            var engine = new ChessBoardEngine<ChessPieceEntity>(new ChessBoardSetup(), 
                new ChessPathsValidator(new ChessPathValidator(new MoveValidationFactory<ChessPieceEntity>()), new BoardActionFactory<ChessPieceEntity>()),
                    new ChessRefreshAllPaths());

            var startLocation = BoardLocation.At("B2");

            var piece = engine.BoardState.GetItem(startLocation);

            Assert.That(piece.Item.EntityType, Is.EqualTo(ChessPieceName.Pawn));

            var paths = piece.Paths;
            Assert.That(paths.Count(), Is.EqualTo(1));
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