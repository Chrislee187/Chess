using System.Collections.Generic;
using System.Linq;
using Chess.Common.Extensions;
using Chess.Common.System;

namespace Chess.Common.Movement
{
    ///  <summary>
    ///  Move Generators are expected to generate a list of the available moves to a Piece on a supplied board.
    ///  
    ///  Even though the move generator has access to the board they are not expected to take any check states 
    ///  into account (i.e. King moves generated may put a king in check). 
    ///  
    ///  MoveGenerator results should only be made available through an instance of <see cref="Common.Board"/> which 
    ///  will filter them against the current state of the board.
    ///  
    ///  <remarks>Although it would be nice to have the moves work out whether they involve check themselves, it makes the
    ///  generation of each pieces movelist more problematic as to generate to the move list of a piece, you need the move list of all other pieces
    ///  to ensure the move doesn't uncover check.
    /// 
    ///  Therefore the board will be resposible for managing this behaviour. See <see cref="Common.Board.RemoveMovesThatLeaveBoardInCheck"/>
    ///  </remarks>
    ///  </summary>
    public abstract class MoveGeneratorBase : IMoveGenerator
    {
        public virtual IEnumerable<Move> All(Common.Board board, BoardLocation at)
        {

            return ValidMoves(board, at)
                .Concat(ValidTakes(board, at))
                .Concat(ValidCovers(board, at))
                .ToList();
        }
        /// <summary>
        /// "Moves" are normal non-taking moves, includes castling, pawn promotions, etc.
        /// </summary>
        protected abstract IEnumerable<Move> ValidMoves(Common.Board board, BoardLocation at);
        /// <summary>
        /// "Takes" are normal taking moves
        /// </summary>
        protected abstract IEnumerable<Move> ValidCovers(Common.Board board, BoardLocation at);
        /// <summary>
        /// "Covers" are NOT actual moves but a list of any locations containing friendly pieces that
        /// this piece could attack.
        /// </summary>
        protected abstract IEnumerable<Move> ValidTakes(Common.Board board, BoardLocation at);

        protected delegate bool DestinationCheck(Common.Board board, BoardLocation from, BoardLocation to);

        protected static IEnumerable<Move> AddTransformationsIf(Common.Board board,
            BoardLocation from,
            DestinationCheck destinationCheck,
            MoveType moveType,
            IEnumerable<LocationFactory> directions)
            => LocationFactory.ApplyToMany(from, directions)
                .Where(to => destinationCheck(board, from, to))
                .Select(m => new Move(@from, m, moveType));
    }
}