using chess.blazor.Shared.Chess;
using Microsoft.AspNetCore.Components;
using Moq;
using NUnit.Framework;

namespace chess.blazor.tests
{
    [TestFixture]
    public class BoardCellComponentTests
    {
        private BoardCellComponent _component;

        [SetUp]
        public void SetUp()
        {
            _component = new BoardCellComponent();
        }

        private const string AnyMove = "a1a2";

        [TestCase(' ')]
        [TestCase('.')]
        public void IsEmptySquare_returns_true(char piece)
        {
            _component.Piece = piece;

            Assert.True(_component.IsEmptySquare);
        }

        [Test]
        public void OnClick_uses_OnPieceSelected_callback()
        {
            // TODO: Find how to test this once I get my internet back:(
//            var handleEvent = new Mock<IHandleEvent>();
//            var x = new EventCallback<PieceSelectedEventArgs>();
//
//            _component.OnPieceSelected = x;
//
//            _component.OnClick();
            Assert.Inconclusive();
        }
    }
}