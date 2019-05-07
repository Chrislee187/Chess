using System;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement.Validators;
using NUnit.Framework;

namespace chess.engine.tests.Chess
{

    [TestFixture]
    public class ChessGameStateTests
    {
        [Test]
        public void Should_find_simple_check_condition()
        {
            var result = new ChessGameState()
                .CurrentGameState(new EasyBoardBuilder()
                    .Board("R   k   " +
                           "        " +
                           "        " +
                           "        " +
                           "        " +
                           "        " +
                           "        " +
                           "    K  R"
                    ).ToBoardState(), Colours.Black);

            Assert.That(result, Is.EqualTo(GameState.Check));
        }
        [Test]
        public void Should_find_simple_checkmate_condition()
        {
            var boardState = new EasyBoardBuilder()
                .Board("R   k   " +
                       "       R" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K   "
                ).ToBoardState();
            var result = new ChessGameState()
                .CurrentGameState(boardState, Colours.Black);

            Assert.That(result, Is.EqualTo(GameState.Checkmate));
        }
    }
}