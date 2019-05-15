using System;
using System.Runtime.Serialization;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Algebraic;
using chess.engine.Extensions;
using chess.engine.Game;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace chess.engine.tests.Algebraic
{
    [TestFixture]
    public class SanMoveFinderTests
    {
        [TestCase("d4", "D2D4")]
        [TestCase("Na3", "B1A3")]
        public void Should_find(string sanText, string expectedMoveText)
        {
            var game = ChessFactory.NewChessGame();

            var move = new SanMoveFinder(game.BoardState)
                .Find(sanText.ToSan(), Colours.White);

            Assert.NotNull(move, "No move found!");
            Assert.That(move.ToChessCoords(), Is.EqualTo(expectedMoveText));
        }

        [TestCase("axb4", "A3B4", Colours.White)]
        [TestCase("Bfxg7", "F8G7", Colours.Black)]
        public void ShouldDisambiguateFile(string sanText, string expectedMoveText, Colours toPlay)
        {
            var builder = new ChessBoardBuilder()
                .Board("r   kb b" +
                       "      P " +
                       "        " +
                       "        " +
                       " p      " +
                       "P       " +
                       "        " +
                       "B B K  R"
                );

            var game = ChessFactory.CustomChessGame(builder.ToGameSetup(), toPlay);
            var move = new SanMoveFinder(game.BoardState)
                .Find(sanText.ToSan(), toPlay);

            Assert.NotNull(move, "No move found!");
            Assert.That(move.ToChessCoords(), Is.EqualTo(expectedMoveText));
        }
    }

}