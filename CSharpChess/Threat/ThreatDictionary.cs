using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.Threat
{
    public class ThreatDictionary
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public BoardLocation Location { get; }
        // ReSharper disable once MemberCanBePrivate.Global
        public IEnumerable<ChessMove> All { get; }
        // ReSharper disable once MemberCanBePrivate.Global
        public Chess.Board.Colours LocationOwner { get; }
        public IEnumerable<ChessMove> Moves => All.Where(m => m.MoveType.IsMove());
        public IEnumerable<ChessMove> Takes => All.Where(m => m.MoveType.IsTake());
        public IEnumerable<ChessMove> Covers => All.Where(m => m.MoveType.IsCover());
        public IEnumerable<ChessMove> Threats { get; } = new List<ChessMove>();

        public ThreatDictionary(BoardLocation location, IEnumerable<ChessMove> all, IEnumerable<ChessMove> threats,
            Chess.Board.Colours locationOwner)
        {
            Location = location;
            All = all;
            Threats = threats;
            LocationOwner = locationOwner;
        }

        public ThreatDictionary(Chess.Board.Colours locationOwner)
        {
            LocationOwner = locationOwner;
        }
    }
}