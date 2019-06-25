using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using chess.blazor.Shared.Chess;
using chess.webapi.client.csharp;
using Microsoft.AspNetCore.Components;

namespace chess.blazor.Pages
{
    public class BoardTestComponent : ComponentBase
    {
        [Parameter] public bool WhiteIsHuman { get; set; } = true;
        [Parameter] public bool BlackIsHuman { get; set; } = false;

        protected ChessBoardComponent ChessBoard { get; set; }
        protected AvailableMoveListComponent MoveList { get; set; }
        [Inject] public IChessGameApiClient ApiClient { get; set; }

        private ChessWebApiResult _firstResult;
        private ChessWebApiResult _lastResult;

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
        }

        private void UpdateBoardAndMoves(ChessWebApiResult result)
        {
            UpdateChessBoardComponent(result);

            UpdateMoveListComponent(result);

            Console.WriteLine("Triggering state change");
            StateHasChanged();
        }

        private void UpdateMoveListComponent(ChessWebApiResult result)
        {
            Console.WriteLine("Updating movelist...");
            MoveList.Moves = result.AvailableMoves;
            MoveList.ShowMoveList = !IsAITurn(result);

            Console.WriteLine("Updating message...");
            if (string.IsNullOrEmpty(result.Message))
            {
                MoveList.Title = $"{result.WhoseTurn} to play";
            }
            else
            {
                MoveList.Title = result.Message;
            }
        }

        private void UpdateChessBoardComponent(ChessWebApiResult result)
        {
            Console.WriteLine("Updating board...");
            ChessBoard.WhiteToPlay = result.WhoseTurn.ToLower().Contains("white");
            ChessBoard.Board = result.Board;
        }

        public async Task OnMoveSelectedAsync(string move)
        {
            ChessBoard.Message = "";
            Console.WriteLine($"OnMoveSelectedAsync({move})");
            try
            {
                _lastResult = await ApiClient.PlayMoveAsync(ChessBoard.Board, EncodeMove(move));
                UpdateBoardAndMoves(_lastResult);

                await HandleAIPlayer(_lastResult);

            }
            catch (Exception e)
            {
                ChessBoard.Message = $"Error performing move"; // TODO: This hides all errors not just invalid moves.
            }


        }

        private async Task HandleAIPlayer(ChessWebApiResult lastResult)
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
            StateHasChanged();
            Thread.Sleep(1000);
            var rnd = new Random().Next(1, lastResult.AvailableMoves.Length + 1);
            await OnMoveSelectedAsync(lastResult.AvailableMoves[rnd].Coord);
        }

        public async Task ResetBoardAsync()
        {
            await InitialiseBoardAsync();
        }

        public string EncodeMove(string move) => move.Replace("+", "");
    }
}