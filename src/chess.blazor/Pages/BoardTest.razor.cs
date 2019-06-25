using System;
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

            UpdateBoardAndMoves(_firstResult);
        }

        private void UpdateBoardAndMoves(ChessWebApiResult result)
        {
            ChessBoard.WhiteToPlay = result.WhoseTurn.ToLower().Contains("white");
            ChessBoard.Board = result.Board;
            Console.WriteLine(ChessBoard.WhiteToPlay);
            MoveList.Moves = result.AvailableMoves;
            if (string.IsNullOrEmpty(result.Message))
            {
                MoveList.Title = $"{result.WhoseTurn} to play";
            }
            else
            {
                MoveList.Title = result.Message;
            }
        }

        public async Task OnMoveSelectedAsync(string move)
        {
            Console.WriteLine($"OnMoveSelectedAsync({move})");
            var result = await ApiClient.PlayMoveAsync(ChessBoard.Board, EncodeMove(move));
            UpdateBoardAndMoves(result);
        }

        public async Task ResetBoardAsync()
        {
            await InitialiseBoardAsync();
        }

        public string EncodeMove(string move) => move.Replace("+", "");
    }
}