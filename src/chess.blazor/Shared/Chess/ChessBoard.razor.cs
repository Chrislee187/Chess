using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chess.webapi.client.csharp;
using Microsoft.AspNetCore.Components;

namespace chess.blazor.Shared.Chess
{
    public class ChessBoardComponent : ComponentBase
    {
        private readonly IDictionary<string, BoardCellComponent> _cells = new Dictionary<string, BoardCellComponent>();
        private readonly MoveSelection _moveSelection;

        public ChessBoardComponent()
        {
            _moveSelection = new MoveSelection(_cells);
        }

        public BoardCell BoardCell
        {
            set
            {
                var key = $"{(char) (value.X + _asciiLowerCaseA - 1)}{value.Y}";
                _cells[key] = value;
            }
        }

        [Parameter]
        public string Board
        {
            get => _emptyBoard;
            set
            {
                _emptyBoard = value;
                StateHasChanged();
            }
        }
        public Move[] AvailableMoves { get; set; }

        [Parameter]
        private EventCallback<string> OnMoveSelectedAsync { get; set; }

        public async Task<bool> MoveSelected(string move)
        {
            Console.WriteLine($"Making move: {move}");
            if (OnMoveSelectedAsync.HasDelegate) await OnMoveSelectedAsync.InvokeAsync(move);
            _moveSelection.Clear();
            return true;
        }

        public char Piece(int x, int y) => Board[ToBoardStringIdx(x,y)];
        public string Message { get; set; }

        [Parameter]
        public bool WhiteToPlay { get; set; }


        private string _emptyBoard = new string('.', 64);

        private int ToBoardStringIdx(int x, int y) => ((8 - y) * 8) + x - 1;

        private int _asciiLowerCaseA = 'a';
        public async Task PieceSelectedAsync(PieceSelectedEventArgs args)
        {
            _moveSelection.Selected($"{(char)(args.X + _asciiLowerCaseA - 1)}{args.Y}");
            _moveSelection.Updated(AvailableMoves);
            if (_moveSelection.HaveMove)
            {
                await MoveSelected($"{_moveSelection.Move}");
                ClearSourceLocationSelection();
            }

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

        public void Refresh(string resultBoard, Move[] resultAvailableMoves)
        {
            Board = resultBoard;
            AvailableMoves = resultAvailableMoves;

        }
    }
}