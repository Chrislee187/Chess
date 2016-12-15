namespace CSharpChess.TheBoard
{
    public class MoveResult
    {
        public string Message { get; private set; }
        public bool Succeeded { get; }
        public MoveType MoveType { get; }
        public ChessMove Move { get; private set; }

        private MoveResult(bool success, ChessMove move, MoveType moveType)
        {
            Succeeded = success;
            MoveType = moveType;
            Move = move;
        }

        private MoveResult(bool success, ChessMove move, string message) : this(success, move, move.MoveType)
        {
            Message = message;
        }

        public static MoveResult IncorrectPlayer(ChessMove move)
        {
            return new MoveResult(false, move, "Incorrect Player");
        }

        public static MoveResult Success(ChessMove move, MoveType moveType = MoveType.Move)
        {
            return new MoveResult(true, move, moveType);
        }

        public static MoveResult Enpassant(ChessMove move)
        {
            return new MoveResult(true, move, MoveType.TakeEnPassant);
        }

        public static MoveResult Promotion(ChessMove move, MoveType moveType = MoveType.Promotion)
        {
            return new MoveResult(true, move, MoveType.Promotion);
        }

        public static MoveResult Failure(string message, ChessMove move)
        {
            return new MoveResult(false, move, message);
        }
    }

    public enum MoveType
    {
        Move, Take, TakeEnPassant, Castle, Check, Checkmate,
        Promotion,
        Unknown,
        Taken,
        Cover, Invalid
    }

}