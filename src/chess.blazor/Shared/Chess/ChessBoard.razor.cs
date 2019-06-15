using System.Net.Http;
using System.Threading.Tasks;
using chess.blazor.Extensions;
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

        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public IChessGameApiClient ApiClient { get; set; }
        public char Piece(int x, int y) => Board[ToBoardStringIdx(x,y)];

        private string _startingBoard;
        protected override async Task OnInitAsync()
        {
            await ResetBoard();
        }

        private async Task InitialiseBoard()
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(_startingBoard))
            {
                // TODO: Create a proper client, not the nswag one, way to cumbersome for these basic needs
                // use a proper base class/virtuals etc. to handle error flows etc. See the NSwag one for details
                //                result= await HttpClient.GetStringAsync("https://chess-web-api.azurewebsites.net/api/chessgame");

                result = await ApiClient.ChessGameAsync();

                _startingBoard = ExtractBoard(result);
            }

            Board = _startingBoard;
            StateHasChanged();
        }

        private string ExtractBoard(string chessWebApiResult)
        {
            var jsonDocument = System.Text.Json.JsonDocument.Parse(chessWebApiResult);
            return jsonDocument.RootElement.GetString("board");
        }

        private bool _flip;
        public void Test()
        {
            Board = _flip ? Board.ToUpper() : Board.ToLower();
            _flip = !_flip;
        }

        public async Task ResetBoard()
        {
            await InitialiseBoard();
        }

        private int ToBoardStringIdx(int x, int y)
        {
            return ((8 - y) * 8) + x - 1;
        }
    }
}