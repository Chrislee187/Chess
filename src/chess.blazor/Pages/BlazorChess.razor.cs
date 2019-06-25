using System;
using System.Threading.Tasks;
using chess.blazor.Shared.Chess;
using chess.webapi.client.csharp;
using Microsoft.AspNetCore.Components;

namespace chess.blazor.Pages
{
    public class BlazorChessComponent : ComponentBase
    {
        [Parameter] public bool WhiteIsHuman { get; set; } = true;
        [Parameter] public bool BlackIsHuman { get; set; } = false;

        protected ChessBoardComponent ChessBoard { get; set; }
        protected AvailableMoveListComponent MoveList { get; set; }
        [Inject] public IChessGameApiClient ApiClient { get; set; }

        private ChessWebApiResult _firstResult;
        private ChessWebApiResult _lastResult;

        private int _moveCount = 0;
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

            _lastResult = _firstResult ?? throw new NullReferenceException("Unable to initialise board");

            UpdateBoardAndMoves(_firstResult);
            _moveCount = 0;
        }

        private void UpdateBoardAndMoves(ChessWebApiResult result)
        {
            UpdateChessBoardComponent(result);

            UpdateMoveListComponent(result);

            Status("Triggering state change");
        }

        private void UpdateMoveListComponent(ChessWebApiResult result)
        {
            var title = string.IsNullOrEmpty(result.Message)
                ? $"{result.WhoseTurn} to play"
                : result.Message;

            MoveList.Update(title, result.AvailableMoves, !IsAITurn(result));

        }

        private void UpdateChessBoardComponent(ChessWebApiResult result)
        {
            Status("Updating board...");
            ChessBoard.WhiteToPlay = result.WhoseTurn.ToLower().Contains("white");
            ChessBoard.Refresh(result.Board, result.AvailableMoves);
        }

        public async Task OnMoveSelectedAsync(string move)
        {
            ChessBoard.Message = "";

            try
            {
                if (_moveCount > 150) throw new Exception("Move count exceeded");
                _lastResult = await ApiClient.PlayMoveAsync(ChessBoard.Board, EncodeMove(move));
                UpdateBoardAndMoves(_lastResult);
                StateHasChanged();
                _moveCount++;
                await HandleAiPlayer(_lastResult);
            }
            catch (Exception e)
            {
                ChessBoard.Message = $"Error performing move;\n{e.Message}"; // TODO: This hides all errors not just invalid moves.
                StateHasChanged();
            }


        }

        private async Task HandleAiPlayer(ChessWebApiResult lastResult)
        {
            if (IsAITurn(lastResult))
            {
                await PlayRandomMove(lastResult);
            }
        }

        private bool IsAITurn(ChessWebApiResult lastResult)
        {
            return lastResult.WhoseTurn.ToLower() == "black" && !BlackIsHuman
                   || lastResult.WhoseTurn.ToLower() == "white" && !WhiteIsHuman;
        }

        private async Task PlayRandomMove(ChessWebApiResult lastResult)
        {
            MoveList.Title = $"{lastResult.WhoseTurn} is thinking...";
            MoveList.ShowMoveList = false;
            var rnd = new Random().Next(lastResult.AvailableMoves.Length);
            await OnMoveSelectedAsync(lastResult.AvailableMoves[rnd].SAN);
        }

        public async Task ResetBoardAsync()
        {
            await InitialiseBoardAsync();
        }

        public string EncodeMove(string move) => move.Replace("+", "");

        private void Status(string text)
        {
            // TODO: Move this to something on the page
            Console.WriteLine(text);
        }
    }
}