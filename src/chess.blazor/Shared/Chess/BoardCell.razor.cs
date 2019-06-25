using Microsoft.AspNetCore.Components;

namespace chess.blazor.Shared.Chess
{
    public class BoardCellComponent : ComponentBase
    {
        [Parameter]
        public char Piece { get; set; }

        [Parameter]
        public bool IsBlackSquare { get; set; }

        public bool IsEmptySquare => Piece == ' ' || Piece == '.';

        public bool PieceIsWhite => char.IsUpper(Piece);
    }
}