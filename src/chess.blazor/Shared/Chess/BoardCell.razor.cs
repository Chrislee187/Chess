using Microsoft.AspNetCore.Components;

namespace chess.blazor.Shared.Chess
{
    public class BoardCellComponent : ComponentBase
    {
        [Parameter]
        public char Piece { get; set; }

        [Parameter]
        public bool Black { get; set; }
    }
}