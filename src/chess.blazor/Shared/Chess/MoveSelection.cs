using System.Collections.Generic;
using System.Linq;
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

        public void Selected(string location)
        {
            if (string.IsNullOrWhiteSpace(From))
            {
                From = location;
            }
            else
            {
                if (location == From)
                {
                    Clear();
                }
                else
                {
                    To = location;
                }
            }
        }

        public void Updated(Move[] availlableMoves)
        {

            if (HaveFrom)
            {
                HighlightFromCell(_cells, From);

                HighlightDestinationCells(_cells, availlableMoves
                    .Where(m => m.Coord.StartsWith(From))
                    .Select(m => m.Coord.Substring(2)));
            }
            else
            {
                ClearSourceLocationSelection();

            }
        }

        public bool HaveMove => !string.IsNullOrWhiteSpace(From) && !string.IsNullOrWhiteSpace(To);

        public void Clear()
        {
            From = string.Empty;
            To = string.Empty;
        }


        private void ClearSourceLocationSelection()
        {
            _cells.Values.Where(v => v.IsSourceLocation).ToList().ForEach(v =>
            {
                v.IsSourceLocation = false;
            });
            _cells.Values.Where(v => v.IsDestinationLocation).ToList().ForEach(v =>
            {
                v.IsDestinationLocation = false;
            });
        }

        private void HighlightFromCell(IDictionary<string, BoardCellComponent> boardCellComponents,
            string moveSelectionFrom)
        {
            ClearSourceLocationSelection();
            var fromCell = boardCellComponents[moveSelectionFrom];
            fromCell.SetAsSourceLocation();

        }

        private void HighlightDestinationCells(IDictionary<string, BoardCellComponent> boardCellComponents,
            IEnumerable<string> destinations)
        {
            destinations.ToList()
                .ForEach(dest => boardCellComponents[dest].SetAsDestinationLocation()
                );
        }
    }
}