using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using chess.blazor.Extensions;
using chess.webapi.client.csharp;
using Microsoft.AspNetCore.Components;

namespace chess.blazor.Shared.Chess
{
    /// <summary>
    /// Simple component to generate an anchor tags that targets a new tab
    /// </summary>
    public class ChessBoardComponent : ComponentBase
    {
        [Parameter]
        public string Board
        {
            get => _board;
            set
            {
                _board = value;
                StateHasChanged();
            }
        }


        public char Piece(int x, int y) => Board[ToBoardStringIdx(x,y)];
        public string Message { get; set; }


        private bool _flip;
        private string _board = new string('.', 64);

        public void Test()
        {
            Board = _flip ? Board.ToUpper() : Board.ToLower();
            _flip = !_flip;
        }
        private int ToBoardStringIdx(int x, int y) => ((8 - y) * 8) + x - 1;
    }
}