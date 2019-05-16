using System.Linq;
using board.engine.Movement;
using chess.engine.Chess.Movement.ChessPieces.King;
using chess.engine.Extensions;
using chess.engine.Game;
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
            var boardLocation = "E2".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(boardLocation, (int) Colours.White).ToList();

            Assert.That(whitePaths.Count(), Is.EqualTo(8));

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E2").To("E3", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E2").To("F3", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E2").To("F2", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E2").To("F1", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E2").To("E1", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E2").To("D1", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E2").To("D2", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E2").To("D3", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
        }
    }
}