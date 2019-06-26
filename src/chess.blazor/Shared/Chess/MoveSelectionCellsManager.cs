using System.Collections.Generic;
using System.Linq;
using chess.blazor.Extensions;

namespace chess.blazor.Shared.Chess
{
    public class MoveSelectionCellsManager : IMoveSelectionCellsManager
    {
        private readonly IDictionary<string, BoardCellComponent> _cells;

        public MoveSelectionCellsManager(IDictionary<string, BoardCellComponent> cells)
        {
            _cells = cells;
        }
        public bool ContainsPlayerPiece(string location, bool playerIsWhite)
        {
            var cell = _cells[location];
            if (cell.IsEmptySquare) return false;

            return playerIsWhite && cell.PieceIsWhite
                   || !playerIsWhite && !cell.PieceIsWhite;
        }

        public BoardCellComponent Get(string location) 
            => _cells[location];

        public void ClearSourceHighlights() 
            => _cells.Values.Where(v => v.IsSourceLocation).ForEach(v => { v.IsSourceLocation = false; });

        public void HighlightSourceCell(string location)
        {
            ClearSourceHighlights();
            _cells[location].IsSourceLocation = true;
        }

        public void HighlightDestinationCells(IEnumerable<string> destinations)
        {
            ClearDestinationHighlights();
            destinations.ForEach(dest =>
            {
                _cells[dest].IsDestinationLocation = true;
            });
        }

        public void ClearDestinationHighlights() => _cells.Values.Where(v => v.IsDestinationLocation).ForEach(v => { v.IsDestinationLocation = false; });
    }

    public interface IMoveSelectionCellsManager
    {
        BoardCellComponent Get(string location);
        bool ContainsPlayerPiece(string location, bool playerIsWhite);
        void ClearSourceHighlights();
        void ClearDestinationHighlights();
        void HighlightSourceCell(string location);
        void HighlightDestinationCells(IEnumerable<string> destinations);
    }
}