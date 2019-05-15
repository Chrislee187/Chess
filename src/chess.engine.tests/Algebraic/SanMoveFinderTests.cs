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
        [TestCase("d4", "d2d4")]
        [TestCase("Na3", "b1a3")]
        public void Should_find_moves(string sanText, string expectedMoveText)
        {
            var game = ChessFactory.NewChessGame();

            var move = new SanMoveFinder(game.BoardState)
                .Find(sanText.ToSan(), Colours.White);

            Assert.NotNull(move, "No move found!");
            Assert.That(move.ToChessCoords(), Is.EqualTo(expectedMoveText));
        }

        [TestCase("axb4", "a3b4", Colours.White)]
        [TestCase("Bfxg7", "f8g7", Colours.Black)]
        public void Should_disambiguate_file_to_find_moves(string sanText, string expectedMoveText, Colours toPlay)
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

        [TestCase("O-O-O", "e1c1", Colours.White)]
        [TestCase("O-O", "e1g1", Colours.White)]
        [TestCase("0-0-0", "e8c8", Colours.Black)]
        [TestCase("0-0", "e8g8", Colours.Black)]
        public void Should_find_castle_moves(string sanText, string expectedMoveText, Colours toPlay)
        {
            var builder = new ChessBoardBuilder()
                .Board("r   k  r" +
                       "pppppppp" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "PPPPPPPP" +
                       "R   K  R"
                );

            var game = ChessFactory.CustomChessGame(builder.ToGameSetup(), toPlay);
            var move = new SanMoveFinder(game.BoardState)
                .Find(sanText.ToSan(), toPlay);

            Assert.NotNull(move, "No move found!");
            Assert.That(move.ToChessCoords(), Is.EqualTo(expectedMoveText));
        }
    }

}