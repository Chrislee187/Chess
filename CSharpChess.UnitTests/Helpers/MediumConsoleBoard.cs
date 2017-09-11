using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ConsoleStuff.Panels;
using CSharpChess.Extensions;

namespace CSharpChess.UnitTests.Helpers
{
    public class MediumConsoleBoard
    {
        // TODO: Add threats
        private const int PieceCellSize = 3;
        private const int CellBorderWidth = 2;
        private const int ConsoleCellSize = CellBorderWidth + PieceCellSize;
        private readonly Board _board;

        public static string ToString(Board board)
        {
            return new MediumConsoleBoard(board).Build().ToString();
        }

    public MediumConsoleBoard(Board board)
        {
            _board = board;
        }

        public ConsolePanel Build()
        {
            var panels = GetPiecePanels();

            var boardSize = ConsoleCellSize * 8;
            var boardSquares = new ConsolePanel(boardSize, boardSize);

            foreach (var rank in Chess.Ranks.Reverse())
            {
                foreach (var file in Chess.Files)
                {
                    var loc = BoardLocation.At(file, rank);
                    var panel = panels[loc];

                    var panelX = (((int) file -1) * (ConsoleCellSize-1)) + 1;
                    var panelY = ((8-rank) * (ConsoleCellSize-1)) + 1;
                    boardSquares.PrintAt(panelX, panelY, panel);
                }
            }
            return boardSquares;
        }

        private IDictionary<BoardLocation, ConsolePanel> GetPiecePanels()
        {
            IDictionary<BoardLocation, ConsolePanel> panels = new ConcurrentDictionary<BoardLocation, ConsolePanel>();

            foreach (var file in Chess.Files)
            {
                foreach (var rank in Chess.Ranks)
                {
                    var at = BoardLocation.At(file,rank);
                    panels.Add(at, CreateConsoleCell(_board[at]));
                }
            }
            return panels;
        }

        private ConsolePanel CreateConsoleCell(BoardPiece boardPiece)
        {
            var cell = new ConsolePanel(PieceCellSize, PieceCellSize);

            var c = OneCharBoard.ToChar(boardPiece.Piece);
            c = c == '.' ? ' ' : c;
            cell.PrintAt(PieceCellSize/2 + 1, PieceCellSize/2 + 1, c );

            var border = CreateCellBorder();
            border.PrintAt(2, 2, cell);

            return border;
        }

        private static ConsolePanel CreateCellBorder()
        {
            var border = new ConsolePanel(ConsoleCellSize, ConsoleCellSize);
            var edgeWith = PieceCellSize;
            border.PrintAt(1, 1, $"+{'-'.Repeat(edgeWith)}+");

            int i = 0;
            for (; i < edgeWith; i++)
            {
                border.PrintAt(1, 2 + i, $"|{' '.Repeat(edgeWith)}|");
            }
            border.PrintAt(1, i+2, $"+{'-'.Repeat(edgeWith)}+");
            return border;
        }
    }
}