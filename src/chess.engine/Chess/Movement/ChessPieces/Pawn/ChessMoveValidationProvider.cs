using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Movement.ChessPieces.King;

namespace chess.engine.Chess.Movement.ChessPieces.Pawn
{
    public class ChessMoveValidationProvider : MoveValidationProvider<ChessPieceEntity>
    {
        public ChessMoveValidationProvider()
        {
            Validators.Add((int) ChessMoveTypes.KingMove, new BoardMovePredicate<ChessPieceEntity>[]
            {
                (move, boardState) =>
                {
                    var wrap = DestinationIsEmptyOrContainsEnemyValidator<ChessPieceEntity>.Wrap(boardState);
                    return new DestinationIsEmptyOrContainsEnemyValidator<ChessPieceEntity>().ValidateMove(move,
                        wrap);
                },
                (move, boardState) =>
                {
                    var wrap = DestinationNotUnderAttackValidator<ChessPieceEntity>.Wrap(boardState);
                    return new DestinationNotUnderAttackValidator<ChessPieceEntity>().ValidateMove(move, wrap);
                }
            });

            Validators.Add(
                (int) ChessMoveTypes.TakeEnPassant, new BoardMovePredicate<ChessPieceEntity>[]
                {
                    (move, boardState)
                        =>
                    {
                        var wrap = EnPassantTakeValidator.Wrap(boardState);
                        return new EnPassantTakeValidator().ValidateMove(move, wrap);
                    }
                });
            Validators.Add(
                (int) ChessMoveTypes.CastleKingSide, new BoardMovePredicate<ChessPieceEntity>[]
                {
                    (move, boardState)
                        =>
                    {
                        var wrap = KingCastleValidator.Wrap(boardState);
                        return new KingCastleValidator().ValidateMove(move, wrap);

                    }
                });
            Validators.Add(
                (int) ChessMoveTypes.CastleQueenSide, new BoardMovePredicate<ChessPieceEntity>[]
                {
                    (move, boardState)
                        =>
                    {
                        var wrap = KingCastleValidator.Wrap(boardState);
                        return new KingCastleValidator().ValidateMove(move, wrap);
                    }
                });
        }
    }
}