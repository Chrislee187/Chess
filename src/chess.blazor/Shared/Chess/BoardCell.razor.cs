using Microsoft.AspNetCore.Components;

namespace chess.blazor.Shared.Chess
{
    public class BoardCellComponent : ComponentBase
    {
        [Parameter] public int X { get; set; }
        [Parameter] public int Y { get; set; }

        [Parameter]
        public char Piece { get; set; }

        [Parameter] public bool IsBlackSquare { get; set; }
        [Parameter] public bool IsSourceLocation { get; set; }
        [Parameter] public bool IsDestinationLocation { get; set; }

        [Parameter]
        public EventCallback<PieceSelectedEventArgs> OnPieceSelected { get; set; }
        public void OnClick()
        {
            if (OnPieceSelected.HasDelegate)
            {
                OnPieceSelected.InvokeAsync(new PieceSelectedEventArgs
                {
                    X = X,
                    Y = Y,
                    Piece = Piece
                });
            }
        }


        public bool IsEmptySquare => Piece == ' ' || Piece == '.';

        public bool PieceIsWhite => char.IsUpper(Piece);
    }

    public class PieceSelectedEventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Piece { get; set; }
    }
}