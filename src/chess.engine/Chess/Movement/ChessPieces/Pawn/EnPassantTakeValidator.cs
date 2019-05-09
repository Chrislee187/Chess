using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Movement.ChessPieces.King;

namespace chess.engine.Chess.Movement.ChessPieces.Pawn
{
    public class EnPassantTakeValidator : IMoveValidator<ChessPieceEntity> 
    {

        public bool ValidateMove(BoardMove move, IBoardState<ChessPieceEntity> boardState)
        {
            var normalTakeOk = new DestinationContainsEnemyMoveValidator<ChessPieceEntity>().ValidateMove(move, boardState);
            if (normalTakeOk) return true;

            var piece = boardState.GetItems(move.From).Single().Item;

            var passingPieceLocation = move.To.MoveBack(piece.Player);

            if (boardState.IsEmpty(passingPieceLocation)) return false;
            var passingPiece = boardState.GetItems(passingPieceLocation).Single().Item;
            if (passingPiece.Player.Equals(piece.Player)) return false;
            if (!passingPiece.Piece.Equals(ChessPieceName.Pawn)) return false;

            return CheckPawnUsedDoubleMove(move.To);
        }


        private bool CheckPawnUsedDoubleMove(BoardLocation moveTo)
        {
            // ************************
            // TODO: Need to check move count/history to confirm that the pawn we passed did it's double move last turn
            // ************************
            return true;
        }
    }

    public class ChessMoveValidators : MoveValidationProvider<ChessPieceEntity>
    {
        public ChessMoveValidators()
        {
            Validators.Add((int) ChessMoveTypes.KingMove, new BoardMovePredicate<ChessPieceEntity>[]
            {
                (move, boardState) =>
                    new DestinationIsEmptyOrContainsEnemyValidator<ChessPieceEntity>().ValidateMove(move, boardState),
                (move, boardState) =>
                    new DestinationNotUnderAttackValidator<ChessPieceEntity>().ValidateMove(move, boardState)
            });

            Validators.Add(
                (int) ChessMoveTypes.TakeEnPassant, new BoardMovePredicate<ChessPieceEntity>[]
                {
                    (move, boardState)
                        => new EnPassantTakeValidator().ValidateMove(move, boardState)
                });
            Validators.Add(
                (int) ChessMoveTypes.CastleKingSide, new BoardMovePredicate<ChessPieceEntity>[]
                {
                    (move, boardState)
                        => new KingCastleValidator().ValidateMove(move, boardState)
                });
            Validators.Add(
                (int) ChessMoveTypes.CastleQueenSide, new BoardMovePredicate<ChessPieceEntity>[]
                {
                    (move, boardState)
                        => new KingCastleValidator().ValidateMove(move, boardState)
                });
        }
    }

}