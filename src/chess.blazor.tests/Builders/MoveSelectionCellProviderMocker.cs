using System.Collections.Generic;
using chess.blazor.Shared.Chess;
using Moq;

namespace chess.blazor.tests.Builders
{
    public class MoveSelectionCellProviderMocker
    {
        private readonly Mock<IMoveSelectionCellsManager> _cellsProviderMock = new Mock<IMoveSelectionCellsManager>();
        public IMoveSelectionCellsManager Instance() => _cellsProviderMock.Object;

        public void SetupContainsPlayerPiece(bool value) =>
            _cellsProviderMock.Setup(x
                    => x.ContainsPlayerPiece(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(value);

        public void SetupGet(BoardCellComponent boardCellComponent, string location = "")
        {
            var setup = string.IsNullOrWhiteSpace(location)
                ? _cellsProviderMock.Setup(x => x.Get(It.IsAny<string>()))
                : _cellsProviderMock.Setup(x => x.Get(It.Is<string>(s => s.Equals(location))));

            setup.Returns(boardCellComponent);
        }

        public void SetupGetFail() 
            => _cellsProviderMock.Setup(x => x.Get(It.IsAny<string>()))
                .Returns((BoardCellComponent)null);

        public void VerifySourceCellIsSet(string location)
            => _cellsProviderMock.Verify(m => m.HighlightSourceCell(location));

        public void VerifySomeDestinationCellsWereSet() =>
            _cellsProviderMock.Verify(m
                => m.HighlightDestinationCells(It.IsAny<IEnumerable<string>>()));

        public void VerifySourceCleared()
            => _cellsProviderMock.Verify(m => m.ClearSourceHighlights());

        public void VerifyDestinationsCleared()
            => _cellsProviderMock.Verify(m => m.ClearDestinationHighlights());

        public void VerifySourceCellWasNotSet()
            => _cellsProviderMock.Verify(m => m.HighlightSourceCell(It.IsAny<string>()), Times.Never);

        public void VerifyDestinationCellsWereNotSet() =>
            _cellsProviderMock.Verify(m
                => m.HighlightDestinationCells(It.IsAny<IEnumerable<string>>()), Times.Never);

    }
}