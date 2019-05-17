using System;
using chess.engine.Chess;
using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.tests
{
    [TestFixture]
    public class FullGameParsingTests
    {
        [Test]
        [Explicit("These are slow at the moment, only run the san one for smoke test purposes")]
        public void Should_play_the_wiki_game_with_coord_moves()
        {
            var game = ChessFactory.NewChessGame();
            var moveIdx = 0;
            foreach (var move in TestGameMoveLists.WikiGameCoords)
            {
                Console.WriteLine($"Move #{++moveIdx}: {move}");
                var msg = game.Move(move);
                Console.WriteLine(game.PlayerState + " - " + game.CurrentPlayer + " to move.");
                Console.WriteLine(game.ToText());
                if (!string.IsNullOrEmpty(msg))
                {
                    if (msg.Contains("Error:")) Assert.Fail($"Error: {msg}");
                    Console.WriteLine(msg);
                }
            }

            Console.WriteLine("GAME OVER!");
        }
        [Test]
        public void Should_play_the_wiki_game_with_san_moves()
        {
            var game = ChessFactory.NewChessGame();
            var moveIdx = 0;
            foreach (var move in TestGameMoveLists.WikiGameSan)
            {
                Console.WriteLine($"Move #{++moveIdx}: {move}");
                var msg = game.Move(move);
                Console.WriteLine(game.PlayerState + " - " + game.CurrentPlayer + " to move.");
                Console.WriteLine(game.ToText());
                if (!string.IsNullOrEmpty(msg))
                {
                    if (msg.Contains("Error:")) Assert.Fail($"Error: {msg}");
                    Console.WriteLine(msg);
                }
            }

            Console.WriteLine("GAME OVER!");
        }

        public class TestGameMoveLists
        {
            /// <summary>
            /// Moves taken from https://en.wikipedia.org/wiki/Portable_Game_Notation
            /// </summary>
            public static string[] WikiGameSan =
            {
                "e4",
                "e5",
                "Nf3",
                "Nc6",
                "Bb5",
                "a6",
                "Ba4",
                "Nf6",
                "O-O",
                "Be7",
                "Re1",
                "b5",
                "Bb3",
                "d6",
                "c3",
                "O-O",
                "h3",
                "Nb8",
                "d4",
                "Nbd7",
                "c4",
                "c6",
                "cxb5",
                "axb5",
                "Nc3",
                "Bb7",
                "Bg5",
                "b4",
                "Nb1",
                "h6",
                "Bh4",
                "c5",
                "dxe5",
                "Nxe4",
                "Bxe7",
                "Qxe7",
                "exd6",
                "Qf6",
                "Nbd2",
                "Nxd6",
                "Nc4",
                "Nxc4",
                "Bxc4",
                "Nb6",
                "Ne5",
                "Rae8",
                "Bxf7+",
                "Rxf7",
                "Nxf7",
                "Rxe1+",
                "Qxe1",
                "Kxf7",
                "Qe3",
                "Qg5",
                "Qxg5",
                "hxg5",
                "b3",
                "Ke6",
                "a3",
                "Kd6",
                "axb4",
                "cxb4",
                "Ra5",
                "Nd5",
                "f3",
                "Bc8",
                "Kf2",
                "Bf5",
                "Ra7",
                "g6",
                "Ra6+",
                "Kc5",
                "Ke1",
                "Nf4",
                "g3",
                "Nxh3",
                "Kd2",
                "Kb5",
                "Rd6",
                "Kc5",
                "Ra6",
                "Nf2",
                "g4",
                "Bd3",
                "Re6"
            };

            /// <summary>
            /// Moves taken from https://en.wikipedia.org/wiki/Portable_Game_Notation, manually converted to co-ord format
            /// </summary>
            public static readonly string[] WikiGameCoords = new[]
            {
                "e2e4",
                "e7e5",
                "g1f3",
                "b8c6",
                "f1b5",
                "a7a6",
                "b5a4",
                "g8f6",
                "e1g1",
                "f8e7",
                "f1e1",
                "b7b5",
                "a4b3",
                "d7d6",
                "c2c3",
                "e8g8",
                "h2h3",
                "c6b8",
                "d2d4",
                "b8d7",
                "c3c4",
                "c7c6",
                "c4b5",
                "a6b5",
                "b1c3",
                "c8b7",
                "c1g5",
                "b5b4",
                "c3b1",
                "h7h6",
                "g5h4",
                "c6c5",
                "d4e5",
                "f6e4",
                "h4e7",
                "d8e7",
                "e5d6",
                "e7f6",
                "b1d2",
                "e4d6",
                "d2c4",
                "d6c4",
                "b3c4",
                "d7b6",
                "f3e5",
                "a8e8",
                "c4f7",
                "f8f7",
                "e5f7",
                "e8e1",
                "d1e1",
                "g8f7",
                "e1e3",
                "f6g5",
                "e3g5",
                "h6g5",
                "b2b3",
                "f7e6",
                "a2a3",
                "e6d6",
                "a3b4",
                "c5b4",
                "a1a5",
                "b6d5",
                "f2f3",
                "b7c8",
                "g1f2",
                "c8f5",
                "a5a7",
                "g7g6",
                "a7a6",
                "d6c5",
                "f2e1",
                "d5f4",
                "g2g3",
                "f4h3",
                "e1d2",
                "c5b5",
                "a6d6",
                "b5c5",
                "d6a6",
                "h3f2",
                "g3g4",
                "f5d3",
                "a6e6"
            };
        }

    }
}