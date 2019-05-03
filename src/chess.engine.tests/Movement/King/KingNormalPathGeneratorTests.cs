using System.Linq;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.King;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.King
{
    [TestFixture]
    public class KingNormalPathGeneratorTests : PathGeneratorTestsBase
    {
        private KingNormalPathGenerator _gen;

        [SetUp]
        public void SetUp()
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
                new PathBuilder().From("E2").To("E3", ChessMoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("F3", ChessMoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("F2", ChessMoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("F1", ChessMoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("E1", ChessMoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("D1", ChessMoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("D2", ChessMoveType.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E2").To("D3", ChessMoveType.KingMove).Build(), Colours.White);
        }
    }
}