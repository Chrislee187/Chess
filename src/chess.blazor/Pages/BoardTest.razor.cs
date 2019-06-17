using System.IO;
using System.Threading.Tasks;
using chess.blazor.Shared.Chess;
using chess.webapi.client.csharp;
using Microsoft.AspNetCore.Components;

namespace chess.blazor.Pages
{
    public class BoardTestComponent : ComponentBase
    {
        protected ChessBoardComponent ChessBoard { get; set; }
        protected AvailableMoveListComponent MoveList { get; set; }
        [Inject] public IChessGameApiClient ApiClient { get; set; }

        private ChessWebApiResult _firstResult;
        protected override async Task OnInitAsync()
        {
            await InitialiseBoardAsync();
        }

        private async Task InitialiseBoardAsync()
        {
            if (_firstResult == null)
            {
                _firstResult = await ApiClient.ChessGameAsync();
            }

            ChessBoard.Board = _firstResult.Board;
            MoveList.Moves = _firstResult.AvailableMoves;
        }

        public async Task ResetBoardAsync()
        {
            await InitialiseBoardAsync();
        }
    }
}