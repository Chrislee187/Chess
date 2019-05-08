using System;
using System.Net.NetworkInformation;
using chess.engine.Board;
using NUnit.Framework;

namespace chess.engine.tests.Serialisation
{
    [TestFixture]
    public class BoardSerialisationTests
    {
        [Test]
        public void Should_serialise_and_deserialise_to_68char_format()
        {
            var chessGame = HelperFactory.NewChessGameNoLoggers;
            var actualNewBoard = ChessGameConvert.Serialise(chessGame);

            var expectedNewBoard = "rnbqkbnrpppppppp................................PPPPPPPPRNBQKBNR" // The board
                                   + "W" // Whose turn
                                   + "0000"; // White Queen/King Black Queen/King castle availability

            Assert.That(actualNewBoard, Is.EqualTo(expectedNewBoard));

            var actualGame = ChessGameConvert.Deserialise(actualNewBoard);

            var actualGameAsString = new EasyBoardBuilder().FromChessGame(actualGame).ToString();
            var expectedGameAsString = new EasyBoardBuilder().FromChessGame(chessGame).ToString();
            Assert.That(actualGameAsString, Is.EqualTo(expectedGameAsString));
            Console.WriteLine(actualGameAsString);
        }
    }
}