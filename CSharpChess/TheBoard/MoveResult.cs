namespace CSharpChess.TheBoard
{
    public class MoveResult
    {
        public bool Succeeded { get; }
        public MoveType MoveType { get; }
        public ChessMove Move { get; private set; }

        private MoveResult(bool success, MoveType moveType, ChessMove move)
        {
            Succeeded = success;
            MoveType = moveType;
            Move = move;
        }

        public static MoveResult IncorrectPlayer(ChessMove move)
        {
            return new MoveResult(false, MoveType.Move, move);
        }

        public static MoveResult Success(ChessMove move)
        {
            return new MoveResult(true, MoveType.Move, move);
        }

        public static MoveResult Enpassant(ChessMove move)
        {
            return new MoveResult(true, MoveType.TakeEnPassant, move);
        }

        public static MoveResult Promotion(ChessMove move)
        {
            return new MoveResult(true, MoveType.Promotion, move);
        }
    }

    public enum MoveType
    {
        Move, Take, TakeEnPassant, Castle, Check, Checkmate,
        Promotion,
        Unknown,
    }

}