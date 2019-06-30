using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Actions
{
    public class EnPassantAction : BoardAction<ChessPieceEntity>
    {
        public EnPassantAction(
            IBoardActionProvider<ChessPieceEntity> provider, 
            IBoardState<ChessPieceEntity> boardState
            ) : base(provider, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.From)) return;

            var piece = BoardState.GetItem(move.From).Item;

            var passedPieceLoc = move.To.MoveBack((Colours) piece.Owner);

            BoardState.Remove(passedPieceLoc);
            ActionProvider.Create((int) DefaultActions.MoveOnly, BoardState).Execute(move);
        }
    }

    public class PawnTwoStepAction : BoardAction<ChessPieceEntity>
    {
        public PawnTwoStepAction(
            IBoardActionProvider<ChessPieceEntity> actionProvider, 
            IBoardState<ChessPieceEntity> boardState
            ) : base(actionProvider, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            var piece = BoardState.GetItem(move.From).Item as PawnEntity;

            piece.TwoStep = true;
            ActionProvider.Create((int)DefaultActions.MoveOnly, BoardState).Execute(move);
        }
    }
}