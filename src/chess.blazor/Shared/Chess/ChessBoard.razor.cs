using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace chess.blazor.Shared.Chess
{
    public class ChessBoardComponent : ComponentBase
    {
        private MoveSelection _moveSelection = new MoveSelection();
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

        [Parameter]
        private EventCallback<string> OnMoveSelectedAsync { get; set; }

        public async Task<bool> MoveSelected(string move)
        {
            Console.WriteLine($"Making move: {moveFrom}{moveTo}");
            if (OnMoveSelectedAsync.HasDelegate) await OnMoveSelectedAsync.InvokeAsync(move);
            ClearSelectedMove();
            return true;
        }
        public char Piece(int x, int y) => Board[ToBoardStringIdx(x,y)];
        public string Message { get; set; }

        [Parameter]
        public bool WhiteToPlay { get; set; }

        private string _emptyBoard = new string('.', 64);

        private int ToBoardStringIdx(int x, int y) => ((8 - y) * 8) + x - 1;

        private string moveFrom = "";
        private string moveTo = "";
        private int asciiLowerCaseA = 'a';
        public async Task PieceSelectedAsync(PieceSelectedEventArgs args)
        {
            _moveSelection.Selected($"{(char)(args.X + asciiLowerCaseA - 1)}{args.Y}");

            if (_moveSelection.HaveMove)
            {
                await MoveSelected($"{_moveSelection.Move}");
                _moveSelection.Clear();
            }

            
        }

        public void ClearSelectedMove()
        {
            moveFrom = string.Empty;
            moveTo = string.Empty;
        }
    }

    public class MoveSelection
    {
        public string From { get; set; }
        public string To { get; set; }

        public string Move => $"{From}{To}";

        public void Selected(string location)
        {
            if (string.IsNullOrWhiteSpace(From))
            {
                From = location;
            }
            else
            {
                To = location;
            }
        }

        public bool HaveMove => !string.IsNullOrWhiteSpace(From) && !string.IsNullOrWhiteSpace(To);

        public void Clear()
        {
            From = string.Empty;
            To = string.Empty;
        }
    }
}