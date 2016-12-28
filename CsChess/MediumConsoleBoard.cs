using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ConsoleStuff.Panels;
using CSharpChess;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CsChess
{
    public partial class MediumConsoleBoard
    {
        private readonly ChessBoard _board;

        private Options _options;
        public MediumConsoleBoard(ChessBoard board)
        {
            _board = board;
        }

        public ConsolePanel Build(Options options = null)
        {
            _options = options ?? new Options();
            var panels = GetPiecePanels();

            var boardSize = ((_options.BorderedCellSize -1) * 8) + 1; // Borders overlap
            var boardSquares = new ConsolePanel(boardSize, boardSize);

            foreach (var rank in Chess.Board.Ranks.Reverse())
            {
                foreach (var file in Chess.Board.Files)
                {
                    var loc = BoardLocation.At(file, rank);
                    var panel = panels[loc];

                    var panelX = (((int)file - 1) * (_options.BorderedCellSize - 1)) + 1;
                    var panelY = ((8 - rank) * (_options.BorderedCellSize - 1)) + 1;
                    boardSquares.PrintAt(panelX, panelY, panel);
                }
            }
            return boardSquares;
        }

        private IDictionary<BoardLocation, ConsolePanel> GetPiecePanels()
        {
            IDictionary<BoardLocation, ConsolePanel> panels = new ConcurrentDictionary<BoardLocation, ConsolePanel>();

            var isBlackSquare = true;
            foreach (var file in Chess.Board.Files)
            {
                foreach (var rank in Chess.Board.Ranks)
                {
                    var at = BoardLocation.At(file, rank);
                    var colour = _options.ColouredSquares ?
                            isBlackSquare ? _options.BlackSquareColour : _options.WhiteSquareColour
                            : null;

                    panels.Add(at, CreateConsoleCell(_board[at], colour));
                    isBlackSquare = !isBlackSquare;
                }
                isBlackSquare = !isBlackSquare;
            }
            return panels;
        }

        private ConsolePanel CreateConsoleCell(BoardPiece boardPiece, ConsoleCellColour colour = null)
        {
            var cell = new BorderedPanel(_options.BorderedCellSize, _options.BorderedCellSize);
            AddRanksAndFiles(cell, boardPiece.Location.File, boardPiece.Location.Rank);
            AddPiece(cell, boardPiece.Piece, _options, colour);

            return cell;
        }

        private static void AddPiece(ConsolePanel border, ChessPiece chessPiece, Options options, ConsoleCellColour colour = null)
        {
            var cell = new ConsolePanel(options.PiecePanelSize, options.PiecePanelSize);
            cell.Fill(' ', colour);

            var c = OneCharBoard.ToChar(chessPiece);
            c = c == '.' ? c = ' ' : c;

            var x = (options.PiecePanelSize / 2) + 1;
            cell.PrintAt(x, x, c, colour);

            var px = (options.BorderedCellSize - options.PiecePanelSize) / 2;
            border.PrintAt(px + 1, px + 1, cell);
        }

        private void AddRanksAndFiles(ConsolePanel border, Chess.Board.ChessFile file, int rank)
        {
            if (_options.ShowRanksAndFiles)
            {
                var midPoint = (_options.BorderedCellSize / 2) + 1;
                if (file == Chess.Board.ChessFile.A)
                {
                    border.PrintAt(1, midPoint, rank.ToString().First());
                }
                if (file == Chess.Board.ChessFile.H)
                {
                    border.PrintAt(_options.BorderedCellSize, midPoint, rank.ToString().First());
                }
                if (rank == 1)
                {
                    border.PrintAt(midPoint, _options.BorderedCellSize, file.ToString());
                }
                if (rank == 8)
                {
                    border.PrintAt(midPoint, 1, file.ToString());
                }
            }
        }
    }
}