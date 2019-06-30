using board.engine;
using chess.engine.Entities;
using chess.engine.Extensions;

namespace chess.engine.Game
{
    public class ChessBoardSetup : IBoardSetup<ChessPieceEntity>
    {
        private readonly IBoardEntityFactory<ChessPieceEntity> _entityFactory;

        public ChessBoardSetup(IBoardEntityFactory<ChessPieceEntity> entityFactory)
        {
            _entityFactory = entityFactory;
        }
        public void SetupPieces(BoardEngine<ChessPieceEntity> engine)
        {
            AddPawns(engine);
            AddMajorPieces(engine);
        }
        
        private void AddPawns(BoardEngine<ChessPieceEntity> engine)
        {
            foreach (var colour in new[] { Colours.White, Colours.Black })
            {
                for (int x = 1; x <= engine.Width; x++)
                {
                    engine.AddPiece(CreatePawn(colour),
                        BoardLocation.At(x, colour == Colours.White ? 2 : 7));
                }
            }
        }

        private void AddMajorPieces(BoardEngine<ChessPieceEntity> engine)
        {
            foreach (var rank in new[] { 1, 8 })
            {
                var colour = rank == 1 ? Colours.White : Colours.Black;

                engine.AddPiece(CreateRook(colour), $"A{rank}".ToBoardLocation());
                engine.AddPiece(CreateKnight(colour), $"B{rank}".ToBoardLocation());
                engine.AddPiece(CreateBishop(colour), $"C{rank}".ToBoardLocation());
                engine.AddPiece(CreateQueen(colour), $"D{rank}".ToBoardLocation());
                engine.AddPiece(CreateKing(colour), $"E{rank}".ToBoardLocation());
                engine.AddPiece(CreateBishop(colour), $"F{rank}".ToBoardLocation());
                engine.AddPiece(CreateKnight(colour), $"G{rank}".ToBoardLocation());
                engine.AddPiece(CreateRook(colour), $"H{rank}".ToBoardLocation());
            }
        }


        private ChessPieceEntity CreatePawn(Colours colour) =>
            _entityFactory.Create(new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData
            {
                Owner = colour,
                PieceName = ChessPieceName.Pawn
            });

        private ChessPieceEntity CreateRook(Colours colour) =>
            _entityFactory.Create(new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData
            {
                Owner = colour,
                PieceName = ChessPieceName.Rook
            });

        private ChessPieceEntity CreateKnight(Colours colour) =>
            _entityFactory.Create(new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData
            {
                Owner = colour,
                PieceName = ChessPieceName.Knight
            });

        private ChessPieceEntity CreateBishop(Colours colour) =>
            _entityFactory.Create(new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData
            {
                Owner = colour,
                PieceName = ChessPieceName.Bishop
            });

        private ChessPieceEntity CreateQueen(Colours colour) =>
            _entityFactory.Create(new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData
            {
                Owner = colour,
                PieceName = ChessPieceName.Queen
            });

        private ChessPieceEntity CreateKing(Colours colour) =>
            _entityFactory.Create(new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData
            {
                Owner = colour,
                PieceName = ChessPieceName.King
            });

    }
}