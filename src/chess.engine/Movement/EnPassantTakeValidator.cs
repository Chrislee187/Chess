using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public class EnPassantTakeValidator : IMoveValidator
    {

        public bool ValidateMove(ChessMove move, BoardState boardState)
        {
            var normalTakeOk = new DestinationContainsEnemyMoveValidator().ValidateMove(move, boardState);

            var piece = boardState.Entities[move.From];

            var passingPieceLocation = move.To.MoveBack(piece.Player);

            if (!boardState.Entities.TryGetValue(passingPieceLocation, out var passingPiece)) return false;
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
    public class PawnPromotionValidator : IMoveValidator
    {

        public bool ValidateMove(ChessMove move, BoardState boardState)
        {
            var piece = boardState.Entities[move.From];
            var moveOk = new DestinationIsEmptyValidator().ValidateMove(move, boardState);

            var destinationIsEndRank = move.To.Rank == ChessGame.EndRankFor(piece.Player);
            var destinationIsEmpty = boardState.GetEntityOrNull(move.To) == null;

            return destinationIsEndRank && destinationIsEmpty;
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