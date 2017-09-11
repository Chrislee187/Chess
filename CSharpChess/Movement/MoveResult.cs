namespace CSharpChess.Movement
{
    public class MoveResult
    {
        public string Message { get; private set; }
        public bool Succeeded { get; }
        public Move Move { get; private set; }

        private MoveResult(bool success, Move move)
        {
            Succeeded = success;
            Move = move;
        }

        private MoveResult(bool success, Move move, string message) : this(success, move)
        {
            Message = message;
        }

        public static MoveResult IncorrectPlayer(Move move) => new MoveResult(false, move, "Incorrect Player");
        public static MoveResult Success(Move move) => new MoveResult(true, move);
        public static MoveResult Enpassant(Move move) => new MoveResult(true, move);
        public static MoveResult Promotion(Move move) => new MoveResult(true, move);
        public static MoveResult Failure(string message, Move move) => new MoveResult(false, move, message);
    }
}