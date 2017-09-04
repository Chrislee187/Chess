using System;

namespace CSharpChess.System
{
    public class InvalidBoardStateException : Exception
    {
        public ChessBoard Board { get; }

        public InvalidBoardStateException(string message, ChessBoard board) : base(message)
        {
            Board = board;
        }

    }
}