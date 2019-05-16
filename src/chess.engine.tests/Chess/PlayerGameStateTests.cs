using chess.engine.Chess;
using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.tests.Chess
{

    [TestFixture]
    public class PlayerGameStateTests
    {
        [Test]
        public void Should_find_simple_check_condition()
        {
            var builder = new ChessBoardBuilder()
                .Board("R   k   " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K  R"
                );
            var result = ChessFactory.CustomChessGame(builder.ToGameSetup(), Colours.Black);

            Assert.That(result.PlayerState, Is.EqualTo(PlayerState.Check));
        }
        [Test]
        public void Should_find_simple_checkmate_condition()
        {
            var result = ChessFactory.CustomChessGame(new ChessBoardBuilder()
                .Board("R   k   " +
                       "       R" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K   "
                ).ToGameSetup(), Colours.Black).PlayerState;

            Assert.That(result, Is.EqualTo(PlayerState.Checkmate));
        }
    }
}