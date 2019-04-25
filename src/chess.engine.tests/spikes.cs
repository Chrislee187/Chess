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
            var engine = new ChessGameEngine();

            var startLocation = BoardLocation.At("B2");
            var whitePawn = ChessPieceEntityFactory.Create(ChessPieceName.Pawn, Colours.White);
            var blackPawn = ChessPieceEntityFactory.Create(ChessPieceName.Pawn, Colours.Black);
            engine
                .AddEntity(whitePawn, startLocation)
                .AddEntity(blackPawn, BoardLocation.At("D5"))
                .AddEntity(whitePawn, BoardLocation.At("C5"));

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

            //            Assert.That(boardPiece.Paths.Count(), Is.EqualTo(2));
            //
            //            Assert.That(engine.PieceAt("A3").Pressure.White.Count, Is.EqualTo(1));
            //            Assert.That(engine.PieceAt("B3").Pressure.White.Count, Is.EqualTo(0));
            //            Assert.That(engine.PieceAt("C3").Pressure.White.Count, Is.EqualTo(1));
        }
    }

}