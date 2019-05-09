using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Movement.ChessPieces.King;
using chess.engine.Chess.Movement.ChessPieces.Pawn;

namespace chess.engine.Chess.Movement
{
    public class ChessMoveValidationProvider : MoveValidationProvider<ChessPieceEntity>
    {
        public ChessMoveValidationProvider()
        {
            Validators.Add((int)ChessMoveTypes.KingMove, new BoardMovePredicate<ChessPieceEntity>[] {
                (move, boardState) => new DestinationIsEmptyOrContainsEnemyValidator<ChessPieceEntity>().ValidateMove(move, boardState),
                (move, boardState) => new DestinationNotUnderAttackValidator<ChessPieceEntity>().ValidateMove(move, boardState)});

            Validators.Add((int)ChessMoveTypes.TakeEnPassant,
                new BoardMovePredicate<ChessPieceEntity>[]
                {(move, boardState) => new EnPassantTakeValidator().ValidateMove(move, boardState)});
            Validators.Add((int)ChessMoveTypes.CastleKingSide,
                new BoardMovePredicate<ChessPieceEntity>[]
                { (move, boardState) => new KingCastleValidator().ValidateMove(move, boardState)});
            Validators.Add((int)ChessMoveTypes.CastleQueenSide,
                new BoardMovePredicate<ChessPieceEntity>[]
                    {(move, boardState) => new KingCastleValidator().ValidateMove(move, boardState)});

        }
    }
}