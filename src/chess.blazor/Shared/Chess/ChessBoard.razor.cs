using Microsoft.AspNetCore.Components;

namespace chess.blazor.Shared.Chess
{
    public class ChessBoardComponent : ComponentBase
    {
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


        public char Piece(int x, int y) => Board[ToBoardStringIdx(x,y)];
        public string Message { get; set; }

        private string _emptyBoard = new string('.', 64);

        private int ToBoardStringIdx(int x, int y) => ((8 - y) * 8) + x - 1;
    }
}