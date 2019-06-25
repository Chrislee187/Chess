using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chess.blazor.Extensions;
using chess.webapi.client.csharp;
using Microsoft.AspNetCore.Components;

namespace chess.blazor.Shared.Chess
{
    public class ChessBoardComponent : ComponentBase
    {
        private string _currentBoard = new string('.', 64);
        private readonly MoveSelection _moveSelection;

        protected readonly IDictionary<string, BoardCellComponent> BoardCells = new Dictionary<string, BoardCellComponent>();

        [Parameter]
        public string Board
        {
            get => _currentBoard;
            set
            {
                _currentBoard = value;
                StateHasChanged();
            }
        }

        [Parameter] public bool WhiteToPlay { get; set; }

        [Parameter] private EventCallback<string> OnMoveSelectedAsync { get; set; }

        public Move[] AvailableMoves { get; set; }

        public char Piece(int x, int y) => Board[(x, y).ToBoardStringIdx()];
        public string Message { get; set; }

        // ReSharper disable once UnusedMember.Global - referenced in the razor component
        public BoardCell BoardCell
        {
            set => BoardCells[(value.X, value.Y).ToChessLocation()] = value;
        }

        public ChessBoardComponent()
        {
            _moveSelection = new MoveSelection(BoardCells);
        }
        
        public async Task<bool> MoveSelected(string move)
        {
            Console.WriteLine($"Making move: {move}");
            if (OnMoveSelectedAsync.HasDelegate) await OnMoveSelectedAsync.InvokeAsync(move);
            _moveSelection.Clear();
            return true;
        }
        
        public void Refresh(string resultBoard, Move[] resultAvailableMoves)
        {
            Board = resultBoard;
            AvailableMoves = resultAvailableMoves;
        }

        // ReSharper disable once UnusedMember.Global - referenced in the razor component
        public async Task PieceSelectedAsync(PieceSelectedEventArgs args)
        {
            _moveSelection.Selected((args.X, args.Y). ToChessLocation());
            _moveSelection.Updated(AvailableMoves);
            if (_moveSelection.HaveMove)
            {
                await MoveSelected($"{_moveSelection.Move}");
                _moveSelection.ClearSourceLocationSelection();
            }

        }
    }
}