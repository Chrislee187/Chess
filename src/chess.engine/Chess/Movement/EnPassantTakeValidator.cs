using System.Linq;
using chess.engine.Board;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.Validators;

namespace chess.engine.Chess.Movement
{
    public class EnPassantTakeValidator : IMoveValidator
    {

        public bool ValidateMove(BoardMove move, IBoardState boardState)
        {
            var normalTakeOk = new DestinationContainsEnemyMoveValidator().ValidateMove(move, boardState);

            var piece = boardState.GetItems(move.From).Single().Item;

            var passingPieceLocation = move.To.MoveBack(piece.Player);

            if (boardState.IsEmpty(passingPieceLocation)) return false;
            var passingPiece = boardState.GetItems(passingPieceLocation).Single().Item;
            if (passingPiece.Player == piece.Player) return false;
            if (passingPiece.EntityType != ChessPieceName.Pawn) return false;

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