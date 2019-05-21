using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.tests.Game
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
            var game = ChessFactory.CustomChessGame(builder.ToGameSetup(), Colours.Black);

            Assert.That(game.CheckState, Is.EqualTo(GameCheckState.BlackInCheck));
        }
        [Test]
        public void Should_find_simple_checkmate_condition()
        {
            var game = ChessFactory.CustomChessGame(new ChessBoardBuilder()
                .Board("R   k   " +
                       "       R" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K   "
                ).ToGameSetup(), Colours.Black);

            Assert.That(game.CheckState, Is.EqualTo(GameCheckState.BlackCheckmated));
        }
    }
}