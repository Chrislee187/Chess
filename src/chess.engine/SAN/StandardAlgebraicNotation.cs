using System;
using System.Diagnostics;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Pieces;

namespace chess.engine.SAN
{
    [DebuggerDisplay("{DebuggerDisplayText}")]
    public class StandardAlgebraicNotation
    {
        private string DebuggerDisplayText => ToNotation();

        public enum CastleSide
        {
            None,
            King,
            Queen
        }

        public CastleSide CastleMove;
        public static StandardAlgebraicNotation KingSideCastle => new StandardAlgebraicNotation(CastleSide.King);
        public static StandardAlgebraicNotation QueenSideCastle => new StandardAlgebraicNotation(CastleSide.Queen);

        public SanMoveTypes MoveType { get; }
        public ChessPieceName? PromotionPiece { get; }
        public ChessPieceName Piece { get; }

        // NOTE: Added X/Y because I still keep getting them the wrong way round, I blame the
        // phrase "rank and file" for it! :)
        public int ToFileX { get; }
        public int ToRankY { get; }

        public int? FromFileX { get; }
        public int? FromRankY { get; }

        public bool HaveFrom => FromFileX.HasValue && FromRankY.HasValue;

        public bool InCheck { get; }

        public static StandardAlgebraicNotation Parse(string notation)
        {
            var sanTokenParser = new SanTokenParser();
            var an = new SanBuilder(ChessFactory.CheckDetectionService(), sanTokenParser).BuildFrom(notation);

            if (an.CastleMove == CastleSide.None && an.ToNotation() != notation)
            {
                throw new Exception($"Notation parse mismatch: {notation} != {an.ToNotation()}");
            }

            return an;
        }

        public static bool TryParse(string notation, out StandardAlgebraicNotation an)
        {
            an = null;
            if (string.IsNullOrEmpty(notation)) return false;

            try
            {
                an = Parse(notation);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static StandardAlgebraicNotation
            ParseFromGameMove(IBoardState<ChessPieceEntity> boardState, BoardMove move) =>
            new SanBuilder(ChessFactory.CheckDetectionService(), ChessFactory.SanTokenFactory())
                .BuildFrom(boardState, move);

        public StandardAlgebraicNotation(ChessPieceName piece,
            int? fromFileX, int? fromRankY, int toFileX, int toRankY,
            SanMoveTypes moveType = SanMoveTypes.Move,
            ChessPieceName? promotionPiece = null, bool inCheck = false)
        {
            Piece = piece;
            FromFileX = fromFileX;
            FromRankY = fromRankY;
            ToFileX = toFileX;
            ToRankY = toRankY;
            MoveType = moveType;
            PromotionPiece = promotionPiece;
            InCheck = inCheck;
            CastleMove = CastleSide.None;
        }

        private StandardAlgebraicNotation(CastleSide side)
        {
            Piece = ChessPieceName.King;
            CastleMove = side;
            FromFileX = King.StartPositionFor(Colours.White).X;
            ToFileX = side == CastleSide.Queen ? 3 : 7;
        }

        public string ToNotation()
        {
            if (CastleMove != CastleSide.None)
            {
                return CastleMove == CastleSide.King
                    ? "O-O"
                    : "O-O-O";
            }

            var piece = SanTokenParser.ToSanToken(Piece);

            var from = buildChessLocation(FromFileX, FromRankY);
            var move = MoveType == SanMoveTypes.Take ? SanTokenParser.TakeNotator.ToString() : "";
            var to = buildChessLocation(ToFileX, ToRankY);
            var extra = buildExtra(PromotionPiece);
            var inCheck = InCheck ? SanTokenParser.CheckNotator.ToString() : "";
            return $"{piece}{from}{move}{to}{extra}{inCheck}".Trim();
        }

        private string buildExtra(ChessPieceName? promotionPiece)
        {
            if (promotionPiece.HasValue)
            {
                return
                    $"{SanTokenParser.PromoteNotator.ToString()}{ChessPieceNameMapper.ToChar(promotionPiece.Value, Colours.White)}";
            }

            return string.Empty;
        }

        private string buildChessLocation(int? fromFile, int? fromRank)
        {
            var file = "";
            var rank = "";

            if (fromFile.HasValue)
            {
                var tmp = BoardLocation.At(fromFile.Value, 1);
                file = tmp.ToChessCoord()[0].ToString().ToLower();
            }

            if (fromRank.HasValue)
            {
                var tmp = BoardLocation.At(1, fromRank.Value);
                rank = tmp.ToChessCoord()[1].ToString().ToLower();
            }

            return $"{file}{rank}";
        }
    }
}