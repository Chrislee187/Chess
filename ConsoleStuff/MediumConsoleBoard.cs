using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ConsoleStuff.Panels;
using CSharpChess;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace ConsoleStuff
{
    public class BoardOptions
    {
        public bool ColouredSquares = true;
        public bool ShowRanksAndFiles = true;
    }


    public class MediumConsoleBoard
    {
        private static readonly ConsoleCellColour BlackSquare = new ConsoleCellColour(ConsoleColor.White, ConsoleColor.Black);
        private static readonly ConsoleCellColour WhiteSquare = new ConsoleCellColour(ConsoleColor.Black, ConsoleColor.White);
        private const int PieceCellSize = 3;
        private const int CellBorderWidth = 2;
        private const int ConsoleCellSize = (CellBorderWidth + PieceCellSize);
        private readonly ChessBoard _board;

        public static string ToString(ChessBoard board)
        {
            return new MediumConsoleBoard(board).Build().ToString();
        }

        private BoardOptions _options = new BoardOptions();
        public MediumConsoleBoard(ChessBoard board)
        {
            _board = board;
        }

        public ConsolePanel Build()
        {
            var panels = GetPiecePanels();

            var boardSize = ((ConsoleCellSize -1) * 8) + 1; // Borders overlap
            var boardSquares = new ConsolePanel(boardSize, boardSize);

            foreach (var rank in Chess.Board.Ranks.Reverse())
            {
                foreach (var file in Chess.Board.Files)
                {
                    var loc = BoardLocation.At(file, rank);
                    var panel = panels[loc];

                    var panelX = (((int)file - 1) * (ConsoleCellSize - 1)) + 1;
                    var panelY = ((8 - rank) * (ConsoleCellSize - 1)) + 1;
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
                            isBlackSquare ? BlackSquare : WhiteSquare
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
            var cell = CreateCellWithBorder();
            AddRanksAndFiles(cell, boardPiece.Location.File, boardPiece.Location.Rank);
            AddPiece(cell, boardPiece.Piece, colour);

            return cell;
        }

        private static void AddPiece(ConsolePanel border, ChessPiece chessPiece, ConsoleCellColour colour = null)
        {
            var cell = new ConsolePanel(PieceCellSize, PieceCellSize);
            cell.Fill(' ', colour);

            var c = OneCharBoard.ToChar(chessPiece);
            c = c == '.' ? c = ' ' : c;

            cell.PrintAt(PieceCellSize / 2 + 1, PieceCellSize / 2 + 1, c, colour);

            border.PrintAt(2, 2, cell);
        }

        private void AddRanksAndFiles(ConsolePanel border, Chess.Board.ChessFile file, int rank)
        {
            if (_options.ShowRanksAndFiles)
            {
                if (file == Chess.Board.ChessFile.A)
                {
                    border.PrintAt(1, 3, rank.ToString().First());
                }
                if (file == Chess.Board.ChessFile.H)
                {
                    border.PrintAt(5, 3, rank.ToString().First());
                }
                if (rank == 1)
                {
                    border.PrintAt(3, 5, file.ToString());
                }
                if (rank == 8)
                {
                    border.PrintAt(3, 1, file.ToString());
                }
            }
        }

        private static ConsolePanel CreateCellWithBorder(ConsoleCellColour colour = null)
        {
            var border = new ConsolePanel(ConsoleCellSize, ConsoleCellSize);
            var edgeWith = PieceCellSize;
            var topBar = '-'.Repeat(edgeWith);
            border.PrintAt(1, 1, $"+{topBar}+", colour);

            int i = 0;
            for (; i < edgeWith; i++)
            {
                border.PrintAt(1, 2 + i, $"|{' '.Repeat(edgeWith)}|", colour);
            }
            var bottomBar = '-'.Repeat(edgeWith);
            border.PrintAt(1, i + 2, $"+{bottomBar}+", colour);
            return border;
        }
    }
}