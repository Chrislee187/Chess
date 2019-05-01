using chess.engine.Game;
using chess.engine.Pieces;
using NUnit.Framework;

namespace chess.engine.tests.Pieces
{
    [TestFixture]
    public class KingTests
    {
        [TestCase(Colours.White, "E1")]
        [TestCase(Colours.Black, "E8")]
        public void Should_have_correct_starting_positions(Colours player, string loc)
        {
            Assert.That(King.StartPositionFor(player), Is.EqualTo(BoardLocation.At(loc)));
        }
    }
}