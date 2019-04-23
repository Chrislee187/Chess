using System;

namespace Chess.Common.System
{
    public class InvalidBoardStateException : Exception
    {
        public Board Board { get; }

        public InvalidBoardStateException(string message, Board board) : base(message)
        {
            Board = board;
        }

    }
}