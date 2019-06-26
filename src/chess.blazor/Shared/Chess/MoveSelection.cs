using System.Linq;
using chess.webapi.client.csharp;

namespace chess.blazor.Shared.Chess
{
    public class MoveSelection
    {
        public string From { get; set; } = string.Empty;
        public bool HaveFrom => !string.IsNullOrWhiteSpace(From);
        public string To { get; set; } = string.Empty;
        public bool HaveTo => !string.IsNullOrWhiteSpace(To);

        public string Move => $"{From}{To}";


        private readonly IMoveSelectionCellsManager _cellsManager;

        public MoveSelection(IMoveSelectionCellsManager moveSelectionCellsManager)
        {
            _cellsManager = moveSelectionCellsManager;
        }

        public void Selected(string location, Move[] availableMoves, bool whiteToPlay)
        {
            var selectedCell = _cellsManager.Get(location);

            if (selectedCell == null) return;

            if (string.IsNullOrWhiteSpace(From))
            {
                if (_cellsManager.ContainsPlayerPiece(location, whiteToPlay))
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

                if (!destCell.IsEmptySquare && destCell.PieceIsWhite == _cellsManager.Get(From).PieceIsWhite)
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

            _cellsManager.HighlightSourceCell(From);
            _cellsManager.HighlightDestinationCells(destinations);
        }

        public void Deselect()
        {
            _cellsManager.Get(From).IsSourceLocation = false;
            _cellsManager.ClearSourceHighlights();
            _cellsManager.ClearDestinationHighlights();
            From = string.Empty;
            To = string.Empty;
        }

        public bool HaveMove => !string.IsNullOrWhiteSpace(From) && !string.IsNullOrWhiteSpace(To);
    }
}