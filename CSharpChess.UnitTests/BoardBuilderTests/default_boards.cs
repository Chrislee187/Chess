using System;
using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardBuilderTests
{
    [TestFixture]
    public class default_boards : BoardAssertions
    {
        [Test]
        public void empty_board()
        {
            var board = BoardBuilder.EmptyBoard;

            Assert.True(board.Pieces.All(p => p.Piece.Equals(Chess.Pieces.Blank)));
        }

        [Test]
        public void newgame_board()
        {
            var board = BoardBuilder.NewGame;

            AssertNewGameBoard(board);
        }

        [Test]
        public void custom_board_can_be_built_using_onechar_notation()
        {
            var asOneChar = 
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "........" +
                "........" +
                "PPPPPPPP" +
                "RNBQKBNR";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            AssertNewGameBoard(board);
        }

    }
}