using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ConsoleStuff.Panels;
using CSharpChess;
using CSharpChess.System;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CsChess
{
    public class ConsoleBoardBuilder
    {
        private readonly ChessBoard _board;

        private Options _options;
        public ConsoleBoardBuilder(ChessBoard board)
        {
            _board = board;
        }

        public ConsolePanel Build(Options options = null)
        {
            _options = options ?? new Options();
            var panels = CreateSquarePanels();

            var boardSize = ((_options.BorderedCellSize -1) * 8) + 1; // Borders overlap
            var boardSquares = new ConsolePanel(boardSize, boardSize);

            foreach (var rank in Chess.Ranks.Reverse())
            {
                foreach (var file in Chess.Files)
                {
                    AddPieceToBoard(file, rank, panels, boardSquares);
                }
            }
            return boardSquares;
        }

        private void AddPieceToBoard(Chess.ChessFile file, int rank, IDictionary<BoardLocation, ConsolePanel> panels, ConsolePanel boardSquares)
        {
            var loc = BoardLocation.At(file, rank);
            var panel = panels[loc];

            var panelX = (((int) file - 1) * (_options.BorderedCellSize - 1)) + 1;
            var panelY = ((8 - rank) * (_options.BorderedCellSize - 1)) + 1;
            boardSquares.PrintAt(panelX, panelY, panel);
        }

        private IDictionary<BoardLocation, ConsolePanel> CreateSquarePanels()
        {
            IDictionary<BoardLocation, ConsolePanel> panels = new ConcurrentDictionary<BoardLocation, ConsolePanel>();

            var isBlackSquare = true;
            foreach (var file in Chess.Files)
            {
                foreach (var rank in Chess.Ranks)
                {
                    var at = BoardLocation.At(file, rank);
                    var colour = _options.ColouredSquares ?
                            isBlackSquare ? _options.BlackSquareColour : _options.WhiteSquareColour
                            : null;

                    var square = new ConsoleBoardSquare(_options, _board[at], colour);
                    
                    panels.Add(at, square);
                    isBlackSquare = !isBlackSquare;
                }
                isBlackSquare = !isBlackSquare;
            }
            return panels;
        }
    }
}