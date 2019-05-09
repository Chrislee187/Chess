namespace board.engine
{
    public interface IBoardSetup<TEntity> where TEntity : class, IBoardEntity
    {
        void SetupPieces(BoardEngine<TEntity> engine);
    }
}