using System.Linq;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.Validators;

namespace chess.engine.Chess.Movement.Validators
{
    public class EnPassantTakeValidator : IMoveValidator<ChessPieceEntity> 
    {

        public bool ValidateMove(BoardMove move, IBoardState<ChessPieceEntity> boardState)
        {
            var normalTakeOk = new DestinationContainsEnemyMoveValidator<ChessPieceEntity>().ValidateMove(move, boardState);
            if (normalTakeOk) return true;

            var piece = boardState.GetItems(move.From).Single().Item;

            var passingPieceLocation = move.To.MoveBack((Colours)piece.Owner);

            if (boardState.IsEmpty(passingPieceLocation)) return false;
            var passingPiece = boardState.GetItems(passingPieceLocation).Single().Item;
            if (passingPiece.Owner.Equals(piece.Owner)) return false;
            if (!passingPiece.Piece.Equals(ChessPieceName.Pawn)) return false;

            var enpassantOk = CheckPawnUsedDoubleMove(move.To);

            return normalTakeOk || enpassantOk;
        }


        private bool CheckPawnUsedDoubleMove(BoardLocation moveTo)
        {
            // ************************
            // TODO: Need to check move count/history to confirm that the pawn we passed did it's double move last turn
            // ************************
            return true;
        }
    }
}