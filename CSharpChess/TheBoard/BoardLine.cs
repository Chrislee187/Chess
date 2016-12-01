using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSharpChess.TheBoard
{
    public abstract class BoardLine : IEnumerable<BoardPiece>
    {
        public BoardPiece[] Pieces { get; }
        public int Id { get; }

        protected BoardLine(int id, BoardPiece[] pieceArray)
        {
            ValidateParams(id, pieceArray);

            Pieces = pieceArray;
            Id = id;
        }

        protected abstract void ValidateId(int id);
        private void ValidateParams(int id, BoardPiece[] pieceArray)
        {
            ValidateId(id);
            if (pieceArray.Length != 8)
                throw new MissingFieldException($"'{nameof(pieceArray)}' must contain 8 BoardPiece instances");
        }

        public IEnumerator<BoardPiece> GetEnumerator() => Pieces.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
    public class BoardRank : BoardLine
    {
        public int Rank => Id;

        public BoardRank(int rank, BoardPiece[] pieceArray) : base(rank, pieceArray)
        {
            
        }

        protected override void ValidateId(int rank)
        {
            if (Chess.Validations.InvalidRank(rank))
                throw new ArgumentOutOfRangeException(nameof(rank));
        }

        public override string ToString()
        {
            var s = new string(Pieces.Select(OneCharBoard.ToChar).ToArray());
            return $"{s}";
        }
    }

    public class BoardFile : BoardLine
    {
        public int File => Id;

        public BoardFile(Chess.ChessFile file, BoardPiece[] fileArray) : base((int) file, fileArray)
        {
        }

        public BoardFile(int file, BoardPiece[] fileArray) : base(file, fileArray)
        {
        }

        protected override void ValidateId(int file)
        {
            if (Chess.Validations.InvalidFile(file))
                throw new ArgumentOutOfRangeException(nameof(file));
        }
    }
}