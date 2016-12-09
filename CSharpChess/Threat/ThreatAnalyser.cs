using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.Threat
{
    public class ThreatAnalyser
    {
        private readonly ValidMoveFactory _validMoveFactory = new ValidMoveFactory();

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
                : new ThreatDictionary(Chess.Colours.None);
        }

        private void BuildTable(ChessBoard board)
        {
            ThreatsFor.Clear();

            foreach (var boardPiece in board.Pieces.Where(bp => bp.Piece.IsNot(Chess.PieceNames.Blank)))
            {
                var factory = _validMoveFactory.For[boardPiece.Piece.Name];

                var moves = factory.Moves(board, boardPiece.Location);
                var takes = factory.Takes(board, boardPiece.Location);
                var covers = factory.Covers(board, boardPiece.Location);
                var threats = factory.ValidThreats(board, boardPiece.Location).Select(l => new ChessMove(boardPiece.Location, l, MoveType.Unknown));
                ThreatsFor.Add(boardPiece.Location, new ThreatDictionary(boardPiece.Location, moves, takes, covers, threats, boardPiece.Piece.Colour));
            }
        }

        public class ThreatDictionary 
        {
            public BoardLocation Location { get; }
            public Chess.Colours LocationOwner { get; }
            public IEnumerable<ChessMove> Moves { get; } = new List<ChessMove>();
            public IEnumerable<ChessMove> Takes { get; } = new List<ChessMove>();
            public IEnumerable<ChessMove> Covers { get; } = new List<ChessMove>();
            public IEnumerable<ChessMove> Threats { get; } = new List<ChessMove>();
            public ThreatDictionary(BoardLocation location, IEnumerable<ChessMove> moves, IEnumerable<ChessMove> takes, IEnumerable<ChessMove> covers, IEnumerable<ChessMove> threats, Chess.Colours locationOwner)
            {
                Location = location;
                Moves = moves;
                Takes = takes;
                Covers = covers;
                Threats = threats;
                LocationOwner = locationOwner;
            }

            public ThreatDictionary(Chess.Colours locationOwner)
            {
                LocationOwner = locationOwner;
            }
        }

        public IEnumerable<ChessMove> ThreatsAgainst(Chess.Colours asPlayer, BoardLocation at)
        {
            var enemy = Chess.ColourOfEnemy(asPlayer);

            var enemyThreats = ThreatsFor
                .Where(kvp => kvp.Value.LocationOwner == enemy);

            IEnumerable<ChessMove> moves = enemyThreats
                .SelectMany(kvp => kvp.Value.Moves).Where(m => m.To.Equals(at));
            IEnumerable<ChessMove> takes = enemyThreats
                .SelectMany(kvp => kvp.Value.Takes).Where(m => m.To.Equals(at));

            return moves.Concat(takes);
        }
    }
}