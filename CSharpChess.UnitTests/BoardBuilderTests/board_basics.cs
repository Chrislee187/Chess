using System;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardBuilderTests
{
    [TestFixture]
    public class board_basics
    {
        private ChessBoard _board;

        [SetUp]
        public void SetUp()
        {
            _board = BoardBuilder.EmptyBoard;
        }
        [Test]
        public void can_supply_ranks()
        {

            foreach (var boardRank in _board.Ranks)
            {
                Assert.That(boardRank.Count(), Is.EqualTo(8));   
            }

            Assert.That(_board.Ranks.DistinctBy(br => br.Rank).Count(), Is.EqualTo(8));
        }

        [Test]
        public void can_supply_files()
        {
            foreach (var boardFile in _board.Ranks)
            {
                Assert.That(boardFile.Count(), Is.EqualTo(8));
            }

            Assert.That(_board.Files.DistinctBy(br => br.File).Count(), Is.EqualTo(8));
        }

        [TestCase(0,0)]
        [TestCase(9,9)]
        public void cannot_access_board_squares_out_of_bounds(int file, int rank)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var x = _board[file, rank];
            });
        }
    }
}