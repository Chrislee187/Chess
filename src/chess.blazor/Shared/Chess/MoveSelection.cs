using System;
using System.Collections.Generic;
using System.Linq;
using chess.blazor.Extensions;
using chess.webapi.client.csharp;

namespace chess.blazor.Shared.Chess
{
    public class MoveSelection
    {
        public string From { get; set; }
        public bool HaveFrom => !string.IsNullOrWhiteSpace(From);
        public string To { get; set; }
        public bool HaveTo => !string.IsNullOrWhiteSpace(To);

        public string Move => $"{From}{To}";
        private readonly IDictionary<string, BoardCellComponent> _cells;
        public MoveSelection(IDictionary<string, BoardCellComponent> cells)
        {
            _cells = cells;
        }

        public void Selected(string location, Move[] availableMoves, bool whiteToPlay)
        {
            var selectedCell = _cells[location];
            if (string.IsNullOrWhiteSpace(From))
            {
                if (ContainsCurrentPlayersPiece(whiteToPlay, selectedCell))
                {
                    From = location;
                }
                else
                {
                    return;
                }

            }
            else
            {
                if (location == From)
                {
                    Deselect();
                    return;
                }

                var destCell = selectedCell;

                if (!destCell.IsEmptySquare && destCell.PieceIsWhite == _cells[From].PieceIsWhite)
                {
                    From = location;
                }
                else if (!destCell.IsDestinationLocation)
                {
                    return;
                }
                else
                {
                    To = location;
                }
            }
            var destinations = availableMoves
                .Where(mv => mv.Coord.StartsWith(From))
                .Select(m => m.Coord.Substring(2)).ToList();

            HighlightFromCell();
            HighlightToCells(_cells, destinations);
        }

        public void Deselect()
        {
            _cells[From].IsSourceLocation = false;
            ClearAllHighlights();
            From = string.Empty;
            To = string.Empty;
        }

        private static bool ContainsCurrentPlayersPiece(bool whiteToPlay, BoardCellComponent selectedCell)
        {
            if (selectedCell.IsEmptySquare) return false;

            return whiteToPlay && selectedCell.PieceIsWhite
                   || !whiteToPlay && !selectedCell.PieceIsWhite;
        }

        public bool HaveMove => !string.IsNullOrWhiteSpace(From) && !string.IsNullOrWhiteSpace(To);

        private void HighlightFromCell()
        {
            ClearSourceHighlights();
            _cells[From].IsSourceLocation = true;

        }

        public void ClearAllHighlights()
        {
            ClearSourceHighlights();
            ClearDestinationHighlights();
        }
        private void ClearSourceHighlights()
        {
            _cells.Values.Where(v => v.IsSourceLocation).ForEach(v => { v.IsSourceLocation = false; });
        }

        private void HighlightToCells(IDictionary<string, BoardCellComponent> boardCellComponents,
            IEnumerable<string> destinations)
        {
            ClearDestinationHighlights();
            destinations.ForEach(dest =>
            {
                boardCellComponents[dest].IsDestinationLocation = true;
            });
        }

        private void ClearDestinationHighlights()
        {
            _cells.Values.Where(v => v.IsDestinationLocation).ForEach(v => { v.IsDestinationLocation = false; });
        }
    }
}