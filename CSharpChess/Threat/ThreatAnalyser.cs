using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.Threat
{
    public class ThreatAnalyser
    {
        private readonly ChessBoard _board;
        private readonly ValidMoveFactory _validMoveFactory = new ValidMoveFactory();

        private IDictionary<Chess.Colours, IDictionary<BoardLocation, ThreatDictionary>> ThreatsFor { get; }

        public ThreatAnalyser(ChessBoard board)
        {
            _board = board;
            ThreatsFor = new ConcurrentDictionary<Chess.Colours, IDictionary<BoardLocation, ThreatDictionary>>();

            BuildTable(board);
        }

        public ThreatDictionary For(Chess.Colours colour, BoardLocation location)
        {
            if (ThreatsFor.ContainsKey(colour))
            {
                var t = ThreatsFor[colour];
                if (t.ContainsKey(location))
                {
                    return t[location];
                }
            }
            return new ThreatDictionary();
        }

        private void BuildTable(ChessBoard board)
        {
            ThreatsFor.Clear();

            foreach (Chess.Colours attackerColour in Enum.GetValues(typeof(Chess.Colours)))
            {
                if(attackerColour != Chess.Colours.None)
                    ThreatsFor.Add(attackerColour, BuildThreatDictionaryFor(board, attackerColour));
            }
        }

        private IDictionary<BoardLocation,ThreatDictionary> BuildThreatDictionaryFor(ChessBoard board, Chess.Colours attackerColour)
        {
            var dict = new Dictionary<BoardLocation, ThreatDictionary>();
            foreach (var boardPiece in board.Pieces.Where(bp => bp.Piece.Is(attackerColour)))
            {
                var factory = _validMoveFactory.For[boardPiece.Piece.Name];

                var moves = factory.Moves(board, boardPiece.Location);
                var takes = factory.Takes(board, boardPiece.Location);
                var covers = factory.Covers(board, boardPiece.Location);
                var threats = factory.ValidThreats(board, boardPiece.Location).Select(l => new ChessMove(boardPiece.Location, l, MoveType.Unknown));
                dict.Add(boardPiece.Location, new ThreatDictionary(boardPiece.Location, moves, takes, covers, threats));
            }

            return dict;
        }

        public class ThreatDictionary 
        {
            public BoardLocation Location { get; }
            public IEnumerable<ChessMove> Moves { get; } = new List<ChessMove>();
            public IEnumerable<ChessMove> Takes { get; } = new List<ChessMove>();
            public IEnumerable<ChessMove> Covers { get; } = new List<ChessMove>();
            public IEnumerable<ChessMove> Threats { get; } = new List<ChessMove>();

            public ThreatDictionary(BoardLocation location, IEnumerable<ChessMove> moves, IEnumerable<ChessMove> takes, IEnumerable<ChessMove> covers, IEnumerable<ChessMove> threats)
            {
                Location = location;
                Moves = moves;
                Takes = takes;
                Covers = covers;
                Threats = threats;
            }

            public ThreatDictionary()
            {
            }
        }
    }
}