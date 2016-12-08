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

        private readonly IDictionary<Chess.Colours, ThreatDictionary> _attacksForPlayer;

        public ThreatAnalyser(ChessBoard board)
        {
            _board = board;
            _attacksForPlayer = new ConcurrentDictionary<Chess.Colours, ThreatDictionary>();
        }

        public void BuildTable()
        {
            _attacksForPlayer.Clear();
            _attacksForPlayer.Add(Chess.Colours.White, new ThreatDictionary());
            _attacksForPlayer.Add(Chess.Colours.Black, new ThreatDictionary());

            foreach (var attackerColour in _attacksForPlayer.Keys)
            {
                AddThreatsForPlayer(_attacksForPlayer[attackerColour], attackerColour);
            }
        }

        /// <summary>
        /// Return the BoardLocation instances that the supplied BoardLocation is Threatening
        /// </summary>
        /// <param name="boardLocation"></param>
        /// <returns></returns>
        public IEnumerable<BoardLocation> AttacksFrom(BoardLocation boardLocation) 
            => _attacksForPlayer[_board.ColourOfPiece(boardLocation)][boardLocation];

        /// <summary>
        /// Returns the BoardLocation instances that are placing the supplied BoardLocation under Threat
        /// </summary>
        /// <param name="boardLocation"></param>
        /// <param name="asPlayer"></param>
        /// <returns></returns>
        public IEnumerable<BoardLocation> DefendingAt(BoardLocation boardLocation, Chess.Colours asPlayer) 
            => _attacksForPlayer[Chess.ColourOfEnemy(asPlayer)]
                .Where(v => v.Value.Any(l => l.Equals(boardLocation)))
                .Select(v => v.Key);

        private void AddThreatsForPlayer(ThreatDictionary threatDict, Chess.Colours attackerColour)
        {
            foreach (var boardPiece in _board.Pieces.Where(bp => bp.Piece.Is(attackerColour)))
            {
                var validMoves = _validMoveFactory.GetThreateningMoves(_board, boardPiece.Location).ToList();

                var threateningMoves = new List<BoardLocation>();

                if (validMoves.Any())
                {
                    threateningMoves.AddRange(validMoves);
                }
                threatDict.Add(boardPiece.Location, threateningMoves.ToList());
            }
        }

        private class ThreatDictionary : Dictionary<BoardLocation, IEnumerable<BoardLocation>>
        {

        }
    }
}