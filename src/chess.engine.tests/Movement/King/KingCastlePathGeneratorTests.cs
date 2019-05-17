using System.Linq;
using board.engine.Movement;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement.ChessPieces.King;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.King
{
    [TestFixture]
    public class KingCastlePathGeneratorTests : ChessPathGeneratorTestsBase
    {
        private KingCastlePathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new KingCastlePathGenerator();
        }

        [TestCase(Colours.White)]
        [TestCase(Colours.Black)]
        public void PathsFrom_returns_castle_locations_for_kings(Colours forPlayer)
        {
            var rank = forPlayer == Colours.White ? 1 : 8;
            var boardLocation = $"E{rank}".ToBoardLocation();
            var paths = _gen.PathsFrom(boardLocation, (int) forPlayer).ToList();

            Assert.That(paths.Count(), Is.EqualTo(2));

            AssertPathContains(paths,
                new ChessPathBuilder().From($"E{rank}").To($"G{rank}", (int)ChessMoveTypes.CastleKingSide).Build(), Colours.White);
            AssertPathContains(paths,
                new ChessPathBuilder().From($"E{rank}").To($"C{rank}", (int)ChessMoveTypes.CastleQueenSide).Build(), Colours.White);
        }
    }
}