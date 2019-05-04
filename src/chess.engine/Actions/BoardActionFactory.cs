using System;
using System.Collections.Generic;
using chess.engine.Board;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public interface IBoardActionFactory
    {
        IBoardAction Create(ChessMoveType moveType, IBoardState boardState);
        IBoardAction Create(DefaultActions action, IBoardState boardState);
    }

    public class BoardActionFactory : IBoardActionFactory
    {
        private Dictionary<ChessMoveType, Func<IBoardState, IBoardAction>> _actions;
        private Dictionary<DefaultActions, Func<IBoardState, IBoardAction>> _coreActions;

        public BoardActionFactory()
        {
            _actions = new Dictionary<ChessMoveType, Func<IBoardState, IBoardAction>>
            {
                // Generic
                {ChessMoveType.MoveOnly, (s) => new MoveOnlyAction(this, s) },
                {ChessMoveType.TakeOnly, (s) => new TakeOnlyAction(this, s) },
                {ChessMoveType.MoveOrTake, (s) => new MoveOrTakeAction(this, s) },

                // Chess Specific
                {ChessMoveType.KingMove, (s) => new MoveOrTakeAction(this, s) },
                {ChessMoveType.CastleQueenSide, (s) => new CastleAction(this, s) },
                {ChessMoveType.CastleKingSide, (s) => new CastleAction(this, s) },
                {ChessMoveType.PawnPromotion, (s) => new PawnPromotionAction(this, s) },
                {ChessMoveType.TakeEnPassant, (s) => new EnPassantAction(this, s) }
            };

            _coreActions = new Dictionary<DefaultActions, Func<IBoardState, IBoardAction>>
            {
                {DefaultActions.MoveOnly, (s) => new MoveOnlyAction(this, s) },
                {DefaultActions.TakeOnly, (s) => new TakeOnlyAction(this, s) },
                {DefaultActions.MoveOrTake, (s) => new MoveOrTakeAction(this, s) },
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

        public IBoardAction Create(DefaultActions action, IBoardState boardState)
        {
            if (_coreActions.ContainsKey(action))
            {
                return _coreActions[action](boardState);
            }

            throw new NotImplementedException($"ActionType: {action} not implemented");
        }
    }

    public enum DefaultActions
    {
        MoveOnly, TakeOnly, MoveOrTake
    }
}