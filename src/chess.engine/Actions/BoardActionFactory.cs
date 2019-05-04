using System;
using System.Collections.Generic;
using chess.engine.Board;
using chess.engine.Chess.Actions;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public interface IBoardActionFactory
    {
        IBoardAction Create(MoveType moveType, IBoardState boardState);
        IBoardAction Create(DefaultActions action, IBoardState boardState);
    }

    public class BoardActionFactory : IBoardActionFactory
    {
        private Dictionary<MoveType, Func<IBoardState, IBoardAction>> _actions;
        private Dictionary<DefaultActions, Func<IBoardState, IBoardAction>> _coreActions;

        public BoardActionFactory()
        {
            _actions = new Dictionary<MoveType, Func<IBoardState, IBoardAction>>
            {
                // Generic
                {MoveType.MoveOnly, (s) => new MoveOnlyAction(this, s) },
                {MoveType.TakeOnly, (s) => new TakeOnlyAction(this, s) },
                {MoveType.MoveOrTake, (s) => new MoveOrTakeAction(this, s) },
                {MoveType.UpdatePiece, (s) => new UpdatePieceAction(this, s) },

                // Chess Specific
                {MoveType.KingMove, (s) => new MoveOrTakeAction(this, s) },
                {MoveType.CastleQueenSide, (s) => new CastleAction(this, s) },
                {MoveType.CastleKingSide, (s) => new CastleAction(this, s) },
                {MoveType.TakeEnPassant, (s) => new EnPassantAction(this, s) }
            };

            _coreActions = new Dictionary<DefaultActions, Func<IBoardState, IBoardAction>>
            {
                {DefaultActions.MoveOnly, (s) => new MoveOnlyAction(this, s) },
                {DefaultActions.TakeOnly, (s) => new TakeOnlyAction(this, s) },
                {DefaultActions.MoveOrTake, (s) => new MoveOrTakeAction(this, s) },
                {DefaultActions.UpdatePiece, (s) => new UpdatePieceAction(this, s) },
            };
        }

        public IBoardAction Create(MoveType moveType, IBoardState boardState)
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
        MoveOnly, TakeOnly, MoveOrTake,
        UpdatePiece
    }
}