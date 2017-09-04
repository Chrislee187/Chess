using System.Linq;
using ConsoleStuff.Panels;
using CSharpChess;
using CSharpChess.TheBoard;

namespace CsChess
{
    public class ConsoleBoardSquare : BorderedPanel
    {
        private readonly Options _options;
        private readonly BoardPiece _boardPiece;

        public ConsoleBoardSquare(Options options, BoardPiece boardPiece, ConsoleCellColour colour) 
            : base(options.BorderedCellSize, options.BorderedCellSize)
        {
            _options = options;
            _boardPiece = boardPiece;
            RenderPiece(_boardPiece, colour);
        }


        private void RenderPiece(BoardPiece boardPiece, ConsoleCellColour colour = null)
        {
            AddRankAndFileToEdges(boardPiece.Location.File, boardPiece.Location.Rank);

            var piece = CreatePiecePanel(boardPiece, colour);

            var px = (_options.BorderedCellSize - _options.PiecePanelSize) / 2;
            PrintAt(px + 1, px + 1, piece);
        }

        private ConsolePanel CreatePiecePanel(BoardPiece boardPiece, ConsoleCellColour colour)
        {
            var cell = new FilledConsolePanel(_options.PiecePanelSize, _options.PiecePanelSize, ' ', colour);

            var c = OneCharBoard.ToChar(boardPiece.Piece);
            c = c == '.' ? c = ' ' : c;

            var x = (_options.PiecePanelSize / 2) + 1;
            cell.PrintAt(x, x, c, colour);
            return cell;
        }

        private void AddRankAndFileToEdges(Chess.ChessFile file, int rank)
        {
            if (_options.ShowRanksAndFiles)
            {
                var midPoint = (_options.BorderedCellSize / 2) + 1;
                if (file == Chess.ChessFile.A)
                {
                    PrintAt(1, midPoint, rank.ToString().First());
                }
                if (file == Chess.ChessFile.H)
                {
                    PrintAt(_options.BorderedCellSize, midPoint, rank.ToString().First());
                }
                if (rank == 1)
                {
                    PrintAt(midPoint, _options.BorderedCellSize, file.ToString());
                }
                if (rank == 8)
                {
                    PrintAt(midPoint, 1, file.ToString());
                }
            }
        }

    }
}