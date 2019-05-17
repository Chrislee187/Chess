namespace board.engine.Movement
{
    public interface IMoveValidator<TWrapper>
    {
        bool ValidateMove(BoardMove move, TWrapper wrapper);
    }
}