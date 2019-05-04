using System.Linq;
using chess.engine.Chess.Movement.ChessPieces.King;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.tests.Builders;
using chess.engine.tests.Movement;
using NUnit.Framework;

namespace chess.engine.tests.Chess.Movement.King
{
    [TestFixture]
    public class KingNormalPathGeneratorTests : PathGeneratorTestsBase
    {
        private KingNormalPathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new KingNormalPathGenerator();
        }

        [Test]
        public void PathsFrom_returns_all_directions()
        {
            var boardLocation = BoardLocation.At("E2");
            var whitePaths = _gen.PathsFrom(boardLocation, Colours.White).ToList();

            Assert.That(whitePaths.Count(), Is.EqualTo(8));

            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("E3", MoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("F3", MoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("F2", MoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("F1", MoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("E1", MoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("D1", MoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("D2", MoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("D3", MoveType.KingMove).Build(), Colours.White);
        }
    }
}