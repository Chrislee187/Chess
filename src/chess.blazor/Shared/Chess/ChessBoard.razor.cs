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
        public string Board { get; set; } = new string('.', 64);

        [Inject] public IChessGameApiClient ApiClient { get; set; }

        public char Piece(int x, int y) => Board[ToBoardStringIdx(x,y)];

        private string _startingBoard;
        protected override async Task OnInitAsync()
        {
            await ResetBoard();
        }

        private async Task InitialiseBoard()
        {
            ChessWebApiResult result;
            if (string.IsNullOrEmpty(_startingBoard))
            {
                result = await ApiClient.ChessGameAsync();

                _startingBoard = result.Board;
            }

            Board = _startingBoard;
            StateHasChanged();
        }

        private bool _flip;
        public void Test()
        {
            Board = _flip ? Board.ToUpper() : Board.ToLower();
            _flip = !_flip;
        }

        public async Task ResetBoard() => await InitialiseBoard();

        private int ToBoardStringIdx(int x, int y) => ((8 - y) * 8) + x - 1;
    }
}