using System;
using System.Collections.Generic;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public interface IBoardActionFactory
    {
        IBoardAction Create(ChessMoveType moveType, IBoardState boardState);
    }

    public class BoardActionFactory : IBoardActionFactory
    {
        private Dictionary<ChessMoveType, Func<IBoardState, IBoardAction>> _actions;

        public BoardActionFactory()
        {
            _actions = new Dictionary<ChessMoveType, Func<IBoardState, IBoardAction>>
            {
                // Generic
                {ChessMoveType.MoveOnly, s => new MoveOnlyAction(s, this) },
                {ChessMoveType.TakeOnly, s => new TakeOnlyAction(s, this) },
                {ChessMoveType.MoveOrTake, s => new MoveOrTakeAction(s, this) },

                // Chess Specific
                {ChessMoveType.KingMove, s => new MoveOrTakeAction(s, this) },
                {ChessMoveType.CastleQueenSide, s => new CastleAction(s, this) },
                {ChessMoveType.CastleKingSide, s => new CastleAction(s, this) },
                {ChessMoveType.PawnPromotion, s => new PawnPromotionAction(s, this) },
//                {ChessMoveType.TakeEnPassant, s => new FakeAction(s, this) }
            };
        }

        public IBoardAction Create(ChessMoveType moveType, IBoardState boardState)
        {
            if (_actions.ContainsKey(moveType))
            {
                return _actions[moveType](boardState);
            }

            throw new NotImplementedException($"MoveType: {moveType} not implemented");
        }
    }
}