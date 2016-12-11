using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.Threat
{
    public class ThreatAnalyser
    {
        private static readonly ValidMoveFactory ValidMoveFactory = new ValidMoveFactory();

        private IDictionary<BoardLocation, ThreatDictionary> ThreatsFor { get; }

        public ThreatAnalyser(ChessBoard board)
        {
            ThreatsFor = new ConcurrentDictionary<BoardLocation, ThreatDictionary>();

            BuildTable(board);
        }

        public ThreatDictionary For(BoardLocation location)
        {
            return ThreatsFor.ContainsKey(location) 
                ? ThreatsFor[location] 
                : new ThreatDictionary(Chess.Board.Colours.None);
        }

        private void BuildTable(ChessBoard board)
        {
            ThreatsFor.Clear();

            board.Pieces
                .Where(bp => bp.Piece.IsNot(Chess.Board.PieceNames.Blank)).ToList()
                .ForEach( p => ThreatsFor.Add(p.Location, BuildThreatsFor(board, p)));
        }

        private static ThreatDictionary BuildThreatsFor(ChessBoard board, BoardPiece boardPiece)
        {
            var factory = ValidMoveFactory.For[boardPiece.Piece.Name];

            var moves = factory.All(board, boardPiece.Location);
            var threats =
                factory.ValidThreats(board, boardPiece.Location)
                    .Select(l => new ChessMove(boardPiece.Location, l, MoveType.Unknown));
            return new ThreatDictionary(boardPiece.Location, moves, threats, boardPiece.Piece.Colour);
        }
    }
}