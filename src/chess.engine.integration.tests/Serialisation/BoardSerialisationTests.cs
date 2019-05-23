using System;
using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.integration.tests.Serialisation
{
    [TestFixture]
    public class BoardSerialisationTests
    {
        [Test]
        public void Should_serialise_and_deserialise_to_68char_format()
        {
            var chessGame = ChessFactory.NewChessGame(ChessFactory.LoggerType.Null);
            var actualNewBoard = ChessGameConvert.Serialise(chessGame);

            var expectedNewBoard = "rnbqkbnrpppppppp................................PPPPPPPPRNBQKBNR" // The board
                                   + "W" // Whose turn
                                   + "0000"; // White Queen/King Black Queen/King castle availability

            Assert.That(actualNewBoard, Is.EqualTo(expectedNewBoard));

            var actualGame = ChessGameConvert.Deserialise(actualNewBoard);

            var actualGameAsString = new ChessBoardBuilder().FromChessGame(actualGame).ToTextBoard();
            var expectedGameAsString = new ChessBoardBuilder().FromChessGame(chessGame).ToTextBoard();
            Assert.That(actualGameAsString, Is.EqualTo(expectedGameAsString));
            Console.WriteLine(actualGameAsString);
        }
    }
}