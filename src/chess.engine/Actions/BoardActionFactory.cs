using System;
using System.Collections.Generic;
using chess.engine.Board;
using chess.engine.Chess.Actions;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public interface IBoardActionFactory<TEntity> where TEntity : class, IBoardEntity
    {
        IBoardAction Create(MoveType moveType, IBoardState<TEntity> boardState);
        IBoardAction Create(DefaultActions action, IBoardState<TEntity> boardState);
    }

    public class BoardActionFactory<TEntity> : IBoardActionFactory<TEntity> where TEntity : class, IBoardEntity

    {
        private Dictionary<MoveType, Func<IBoardState<TEntity>, IBoardAction>> _actions;
        private Dictionary<DefaultActions, Func<IBoardState<TEntity>, IBoardAction>> _coreActions;

        public BoardActionFactory()
        {
            _actions = new Dictionary<MoveType, Func<IBoardState<TEntity>, IBoardAction>>
            {
                // Generic
                {MoveType.MoveOnly, (s) => new MoveOnlyAction<TEntity>(this, s) },
                {MoveType.TakeOnly, (s) => new TakeOnlyAction<TEntity>(this, s) },
                {MoveType.MoveOrTake, (s) => new MoveOrTakeAction<TEntity>(this, s) },
                {MoveType.UpdatePiece, (s) => new UpdatePieceAction<TEntity>(this, s) },

                // Chess Specific
                {MoveType.KingMove, (s) => new MoveOrTakeAction<TEntity>(this, s) },
                {MoveType.CastleQueenSide, (s) => new CastleAction<TEntity>(this, s) },
                {MoveType.CastleKingSide, (s) => new CastleAction<TEntity>(this, s) },
                {MoveType.TakeEnPassant, (s) => new EnPassantAction<TEntity>(this, s) }
            };

            _coreActions = new Dictionary<DefaultActions, Func<IBoardState<TEntity>, IBoardAction>>
            {
                {DefaultActions.MoveOnly, (s) => new MoveOnlyAction<TEntity>(this, s) },
                {DefaultActions.TakeOnly, (s) => new TakeOnlyAction<TEntity>(this, s) },
                {DefaultActions.MoveOrTake, (s) => new MoveOrTakeAction<TEntity>(this, s) },
                {DefaultActions.UpdatePiece, (s) => new UpdatePieceAction<TEntity>(this, s) },
            };
        }

        public IBoardAction Create(MoveType moveType, IBoardState<TEntity> boardState)
        {
            if (_actions.ContainsKey(moveType))
            {
                return _actions[moveType](boardState);
            }

            throw new NotImplementedException($"MoveType: {moveType} not implemented");
        }

        public IBoardAction Create(DefaultActions action, IBoardState<TEntity> boardState)
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
        MoveOnly, TakeOnly, MoveOrTake,
        UpdatePiece
    }
}