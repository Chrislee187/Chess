using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Chess.Entities;

namespace chess.engine.Chess.Actions
{
    public class ChessBoardActionProvider : BoardActionFactory<ChessPieceEntity>
    {
        public ChessBoardActionProvider(IBoardEntityFactory<ChessPieceEntity> entityFactory) 
            : base(entityFactory)
        {
            Actions.Add((int)ChessMoveTypes.KingMove, (s) => new MoveOrTakeAction<ChessPieceEntity>(this, s));
            Actions.Add((int)ChessMoveTypes.CastleQueenSide, (s) => new CastleAction<ChessPieceEntity>(this, s));
            Actions.Add((int) ChessMoveTypes.CastleKingSide, (s) => new CastleAction<ChessPieceEntity>(this, s));
            Actions.Add((int) ChessMoveTypes.TakeEnPassant, (s) => new EnPassantAction<ChessPieceEntity>(this, s));
        }

    }
}