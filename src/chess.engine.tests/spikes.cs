using System;
using System.Linq;
using chess.engine.Entities;
using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.tests
{
    [TestFixture]
    public class spikes
    {
        [Test]
        public void Should()
        {
            var engine = new ChessBoardEngine();

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

          
            /*
             * We should have enough now to
             *      render a board
             *      show valid moves for pieces
             *      execute a move
             */
        }
    }

}