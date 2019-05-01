using System;
using System.Collections.Generic;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class BoardActionFactory
    {
        private Dictionary<ChessMoveType, Func<IBoardState, BoardAction>> _actions = new Dictionary<ChessMoveType, Func<IBoardState, BoardAction>>
        {
            // Generic
            {ChessMoveType.MoveOnly, s => new MoveOnlyAction(s) },
            {ChessMoveType.TakeOnly, s => new TakeOnlyAction(s) },
            {ChessMoveType.MoveOrTake, s => new MoveOrTakeAction(s) },

            // Chess Specific
            {ChessMoveType.KingMove, s => new MoveOrTakeAction(s) },
            {ChessMoveType.CastleQueenSide, s => new FakeAction(s) },
            {ChessMoveType.CastleKingSide, s => new FakeAction(s) },
            {ChessMoveType.TakeEnPassant, s => new FakeAction(s) }
        };
        public BoardAction Create(ChessMoveType moveType, IBoardState boardState)
        {
            if (_actions.ContainsKey(moveType))
            {
                return _actions[moveType](boardState);
            }

            throw new NotImplementedException($"MoveType: {moveType} not implemented");
        }
    }
}