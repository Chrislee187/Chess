namespace chess.engine
{
    public interface IGameSetup<TEntity> where TEntity : class, IBoardEntity
    {
        void SetupPieces(ChessBoardEngine<TEntity> engine);
    }
}