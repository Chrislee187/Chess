using chess.blazor.Shared.Chess;
using Microsoft.AspNetCore.Components;

namespace chess.blazor.tests.Builders
{
    public class BoardCellComponentBuilder
    {
        private char _piece = 'K';
        private bool _isSourceLocation = true;
        private bool _isDestinationLocation = false;
        private bool _isBlackSquare = true;

        public BoardCellComponent Build()
        {
            return new BoardCellComponent
            {
                Piece = _piece,
                
                IsSourceLocation = _isSourceLocation,
                IsDestinationLocation = _isDestinationLocation,
                IsBlackSquare = _isBlackSquare,
                OnPieceSelected = new EventCallback<PieceSelectedEventArgs>()
            };
        }

        public BoardCellComponentBuilder WithPiece(char piece)
        {
            _piece = piece;
            return this;
        }

        public BoardCellComponentBuilder WithWhitePiece()
        {
            _piece = 'K';
            return this;
        }

        public BoardCellComponentBuilder WithBlackPiece()
        {
            _piece = 'k';
            return this;
        }

        public BoardCellComponentBuilder WithEmptySquare() => WithPiece(' ');

        public BoardCellComponentBuilder WithIsDestinationLocation()
        {
            _isDestinationLocation = true;
            return this;
        }
    }
}