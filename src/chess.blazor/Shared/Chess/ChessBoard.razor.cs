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
        private readonly MoveSelection _moveSelection;

        protected readonly IDictionary<string, BoardCellComponent> BoardCells =
            new Dictionary<string, BoardCellComponent>();

        private string _board = new string('.', 64);

        [Parameter]
        public string Board
        {
            get => _board;
            set => _board = value.Replace("E", "P").Replace("e", "p");
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
            _moveSelection = new MoveSelection(new MoveSelectionCellsManager(BoardCells));
        }

        public async Task MoveSelectedAsync(string move)
        {
            _moveSelection.Deselect();
            if (OnMoveSelectedAsync.HasDelegate)
            {
                await OnMoveSelectedAsync.InvokeAsync(move);
            }
        }

        public void Update(string resultBoard, Move[] resultAvailableMoves, bool whiteToPlay)
        {
            Board = resultBoard;
            AvailableMoves = resultAvailableMoves;
            WhiteToPlay = whiteToPlay;
        }

        // ReSharper disable once UnusedMember.Global - referenced in the razor component
        public async Task PieceSelectedAsync(PieceSelectedEventArgs args)
        {
            var location = (args.X, args.Y).ToChessLocation();
            _moveSelection.Selected(location, AvailableMoves, WhiteToPlay);

            if (_moveSelection.HaveMove)
            {
                await MoveSelectedAsync($"{_moveSelection.Move}");
            }
        }
    }
}