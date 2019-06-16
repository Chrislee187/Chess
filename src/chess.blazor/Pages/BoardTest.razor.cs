using System.Threading.Tasks;
using chess.blazor.Shared.Chess;
using chess.webapi.client.csharp;
using Microsoft.AspNetCore.Components;

namespace chess.blazor.Pages
{
    public class BoardTestComponent : ComponentBase
    {
        protected ChessBoardComponent ChessBoard { get; set; }
        [Inject] public IChessGameApiClient ApiClient { get; set; }

        private ChessWebApiResult _firstResult;
        protected override async Task OnInitAsync()
        {
            await InitialiseBoard();
        }

        private async Task InitialiseBoard()
        {
            if (_firstResult == null)
            {
                _firstResult = await ApiClient.ChessGameAsync();
            }

            ChessBoard.Board = _firstResult.Board;
        }

        public void ResetBoard()
        {
            InitialiseBoard().RunSynchronously();
        }
    }
}