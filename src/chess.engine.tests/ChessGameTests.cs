using System;
using chess.engine.Chess;
using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.tests
{
    [TestFixture]
    public class ChessGameTests
    {
        [Test]
        public void New_game_should_have_white_as_first_played()
        {
            var game = new ChessGame();

            Assert.That(game.CurrentPlayer, Is.EqualTo(Colours.White));
        }

        [Test]
        public void Move_should_update_current_player_when_valid()
        {
            var game = new ChessGame();

            game.Move("D2D4");
            Assert.That(game.CurrentPlayer, Is.EqualTo(Colours.Black));
        }

        [TestCase(Colours.White, 8)]
        [TestCase(Colours.Black, 1)]
        public void EndRankFor_is_correct_for_both_colours(Colours colour, int rank)
        {
            Assert.That(ChessGame.EndRankFor(colour), Is.EqualTo(rank), $"{colour}");
        }

        [Test]
        public void GameState_some_checks_for_this()
        {
            throw new NotImplementedException();
        }
    }
}