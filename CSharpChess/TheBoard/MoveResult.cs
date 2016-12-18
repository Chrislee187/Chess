namespace CSharpChess.TheBoard
{
    public class MoveResult
    {
        public string Message { get; private set; }
        public bool Succeeded { get; }
        public ChessMove Move { get; private set; }

        private MoveResult(bool success, ChessMove move)
        {
            Succeeded = success;
            Move = move;
        }

        private MoveResult(bool success, ChessMove move, string message) : this(success, move)
        {
            Message = message;
        }

        public static MoveResult IncorrectPlayer(ChessMove move)
        {
            return new MoveResult(false, move, "Incorrect Player");
        }

        public static MoveResult Success(ChessMove move)
        {
            return new MoveResult(true, move);
        }

        public static MoveResult Enpassant(ChessMove move)
        {
            return new MoveResult(true, move);
        }

        public static MoveResult Promotion(ChessMove move)
        {
            return new MoveResult(true, move);
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