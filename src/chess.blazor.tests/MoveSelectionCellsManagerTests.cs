using System.Collections.Generic;
using chess.blazor.Shared.Chess;
using chess.blazor.tests.Builders;
using NUnit.Framework;

namespace chess.blazor.tests
{
    public class MoveSelectionCellsManagerTests
    {
        private MoveSelectionCellsManager _provider;
        private Dictionary<string, BoardCellComponent> _cells;

        [SetUp]
        public void Setup()
        {
            _cells = new Dictionary<string, BoardCellComponent>();
            _provider = new MoveSelectionCellsManager(_cells);
        }
        [Test]
        public void ContainsPlayerPiece_returns_false_when_empty()
        {
            var location = "a3";
            SetupCell(location, new BoardCellComponentBuilder().WithEmptySquare().Build());

            Assert.False(_provider.ContainsPlayerPiece(location, true));
        }

        [Test]
        public void ContainsPlayersPiece_returns_false_when_piece_is_white_but_player_is_black()
        {
            var location = "a3";
            SetupCell(location, new BoardCellComponentBuilder().WithWhitePiece().Build());

            Assert.False(_provider.ContainsPlayerPiece(location, false));
        }

        [Test]
        public void ContainsPlayersPiece_returns_false_when_piece_is_black_but_player_is_white()
        {
            var location = "a3";
            SetupCell(location, new BoardCellComponentBuilder().WithBlackPiece().Build());
            Assert.False(_provider.ContainsPlayerPiece(location, true));
        }

        private void SetupCell(string location, BoardCellComponent cell) => _cells[location] = cell;
    }
}