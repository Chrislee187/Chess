using Microsoft.AspNetCore.Components;

namespace chess.blazor.Shared.Chess
{
    /// <summary>
    /// Simple component to generate an anchor tags that targets a new tab
    /// </summary>
    public class ChessBoardComponent : ComponentBase
    {
        [Parameter]
        public string Board { get; set; } = "rnbqkbnrpppppppp                                PPPPPPPPRNBQKBNR";

        public char Piece(int x, int y) => Board[ToBoardStringIdx(x,y)];

        private bool _flip;
        public void Test()
        {
            Board = _flip ? Board.ToUpper() : Board.ToLower();
            _flip = !_flip;
        }

        public static int ToBoardStringIdx(int x, int y)
        {
            return ((8 - y) * 8) + x - 1;
        }
    }
}