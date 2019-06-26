using System;
using System.Linq;
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

        private int _moveCount;
        private static readonly Random Random = new Random();

        protected override async Task OnInitAsync() => await ResetBoardAsync();

        public async Task ResetBoardAsync()
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

            MoveList.Update(title, result.AvailableMoves, !IsAiTurn(result));

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
                // TODO: Temp hack to stop endless auto-games, need proper stalement & not-enough-material-left checks
                if (_moveCount > 150) throw new Exception("Move limit exceeded");
                _lastResult = await ApiClient.PlayMoveAsync(ChessBoard.Board, EncodeMove(move));
                UpdateBoardAndMoves(_lastResult);
                StateHasChanged();  // NOTE: We call StateHasChanged() because we are in a recursive method when handling AI players and therefore the state doesn't automatically get updated until the stack unwinds
                _moveCount++;
                if(!_lastResult.Message.ToLower().Contains("checkmate"))
                {
                    await HandleAiPlayer(_lastResult);
                }
            }
            catch (Exception e)
            {
                ChessBoard.Message = $"Error performing move;\n{e.Message}"; // TODO: Better exception handling and logging
                StateHasChanged();
            }
        }

        private async Task HandleAiPlayer(ChessWebApiResult lastResult)
        {
            if (IsAiTurn(lastResult))
            {
                await PlayRandomMove(lastResult);
            }
        }

        private bool IsAiTurn(ChessWebApiResult lastResult) 
            => lastResult.WhoseTurn.ToLower() == "black" && !BlackIsHuman
            || lastResult.WhoseTurn.ToLower() == "white" && !WhiteIsHuman;

        private async Task PlayRandomMove(ChessWebApiResult lastResult)
        {
            if(!lastResult.AvailableMoves.Any()) return;

            MoveList.Title = $"{lastResult.WhoseTurn} is thinking...";
            MoveList.ShowMoveList = false;
            var rnd = Random.Next(lastResult.AvailableMoves.Length); // TODO: Ok so it's not really an AI ;)
            await OnMoveSelectedAsync(lastResult.AvailableMoves[rnd].SAN);
        }

        public string EncodeMove(string move) => move.Replace("+", ""); // NOTE: '+' breaks the urls!, it's only cosmetic so just remove it

        private void Status(string text)
        {
            // TODO: Move this to something on the page
            Console.WriteLine(text);
        }
    }
}