using System;
using System.Diagnostics.CodeAnalysis;
using CSharpChess.System;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardBuilderTests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class board_basics
    {
        private ChessBoard _board;

        [SetUp]
        public void SetUp()
        {
            _board = BoardBuilder.NewGame;
        }

        [TestCase(0,0)]
        [TestCase(9,9)]
        public void cannot_access_board_squares_out_of_bounds(int file, int rank)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                // ReSharper disable once UnusedVariable
                var x = _board[(Chess.ChessFile) file, rank];
            });
        }
    }
}