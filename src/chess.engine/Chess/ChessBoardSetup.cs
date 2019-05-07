using System;
using chess.engine.Chess.Entities;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Chess
{
    public class ChessBoardSetup : IBoardSetup<ChessPieceEntity>
    {
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
                    engine.AddPiece(ChessPieceEntityFactory.CreatePawn(colour),
                        BoardLocation.At(x, colour == Colours.White ? 2 : 7));
                }
            }
        }

        private void AddMajorPieces(BoardEngine<ChessPieceEntity> engine)
        {
            foreach (var rank in new[] { 1, 8 })
            {
                var colour = rank == 1 ? Colours.White : Colours.Black;

                engine.AddPiece(ChessPieceEntityFactory.CreateRook(colour), BoardLocation.At($"A{rank}"));
                engine.AddPiece(ChessPieceEntityFactory.CreateKnight(colour), BoardLocation.At($"B{rank}"));
                engine.AddPiece(ChessPieceEntityFactory.CreateBishop(colour), BoardLocation.At($"C{rank}"));
                engine.AddPiece(ChessPieceEntityFactory.CreateQueen(colour), BoardLocation.At($"D{rank}"));
                engine.AddPiece(ChessPieceEntityFactory.CreateKing(colour), BoardLocation.At($"E{rank}"));
                engine.AddPiece(ChessPieceEntityFactory.CreateBishop(colour), BoardLocation.At($"F{rank}"));
                engine.AddPiece(ChessPieceEntityFactory.CreateKnight(colour), BoardLocation.At($"G{rank}"));
                engine.AddPiece(ChessPieceEntityFactory.CreateRook(colour), BoardLocation.At($"H{rank}"));
            }
        }
    }
}