using System;
using System.Collections;
using System.Collections.Generic;
using chess.webapi.client.csharp;
using Microsoft.AspNetCore.Components;

namespace chess.blazor.Shared.Chess
{
    /// <summary>
    /// Simple component to generate an anchor tags that targets a new tab
    /// </summary>
    public class AvailableMoveListComponent : ComponentBase
    {
        private IEnumerable<Move> _moves;

        [Parameter]
        public IEnumerable<Move> Moves // TODO: This should not be the APIClient Move but a specific model for use by the blazor component
        {
            get => _moves;
            set
            {
                _moves = value;
                StateHasChanged();
            }
        }

        public bool MoveSelected(string move)
        {
            Console.WriteLine($"Move: {move}");
            return true;
        }
    }
}