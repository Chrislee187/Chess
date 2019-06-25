using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Entities;

namespace chess.engine.Actions
{
    public class ChessBoardActionProvider : BoardActionProvider<ChessPieceEntity>
    {
        public ChessBoardActionProvider(IBoardEntityFactory<ChessPieceEntity> entityFactory) 
            : base(entityFactory)
        {
            Actions.Add((int)ChessMoveTypes.KingMove, (s) => new MoveOrTakeAction<ChessPieceEntity>(this, s));
            Actions.Add((int)ChessMoveTypes.CastleQueenSide, (s) => new CastleAction<ChessPieceEntity>(this, s));
            Actions.Add((int) ChessMoveTypes.CastleKingSide, (s) => new CastleAction<ChessPieceEntity>(this, s));
            Actions.Add((int) ChessMoveTypes.TakeEnPassant, (s) => new EnPassantAction(this, s));
            Actions.Add((int) ChessMoveTypes.PawnTwoStep, (s) => new PawnTwoStepAction(this, s));
        }

    }
}