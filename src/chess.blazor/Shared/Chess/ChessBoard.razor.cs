using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json.Linq;

namespace chess.blazor.Shared.Chess
{
    /// <summary>
    /// Simple component to generate an anchor tags that targets a new tab
    /// </summary>
    public class ChessBoardComponent : ComponentBase
    {
        [Parameter]
        public string Board { get; set; } = new string('.', 64);

        [Inject] public IChessGameApiClient _gameApi { get; set; }
        [Inject] public HttpClient HttpClient { get; set; }
        public char Piece(int x, int y) => Board[ToBoardStringIdx(x,y)];

        private string _startingBoard;
        protected override async Task OnInitAsync()
        {
            ResetBoard();
        }

        private async void InitialiseBoard()
        {
            if (string.IsNullOrEmpty(_startingBoard))
            {
                // TODO: Create a proper client, not the nswag one, way to cumbersome for these basic needs
                // use a proper base class/virtuals etc. to handle error flows etc. See the NSwag one for details
                // 
                _startingBoard = await HttpClient.GetStringAsync("https://chess-web-api.azurewebsites.net/api/chessgame");
            }
            //            var result = await _gameApi.IndexAsync();
            var jObject = JObject.Parse(_startingBoard);

            Board = jObject["board"].ToString();
            StateHasChanged();
        }

        private bool _flip;
        public void Test()
        {
            Board = _flip ? Board.ToUpper() : Board.ToLower();
            _flip = !_flip;
        }

        public void ResetBoard()
        {
            InitialiseBoard();
        }

        private int ToBoardStringIdx(int x, int y)
        {
            return ((8 - y) * 8) + x - 1;
        }
    }
}