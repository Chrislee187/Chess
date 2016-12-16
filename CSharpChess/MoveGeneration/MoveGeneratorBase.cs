using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.MoveGeneration
{
    ///  <summary>
    ///  Move Generators are expected to generate a list of the available moves to a Piece on a supplied board.
    ///  
    ///  Even though the move generator has access to the board they are not expected to take any check states 
    ///  into account (i.e. King moves generated may put a king in check). 
    ///  
    ///  MoveGenerator results should only be made available through an instance of <see cref="ChessBoard"/> which 
    ///  will filter them against the current state of the board.
    ///  
    ///  <remarks>Although it would be nice to have the moves work out whether they involve check themselves, it makes the
    ///  generation of each pieces movelist more problematic as to generate to the move list of a piece, you need the move list of all other pieces
    ///  to ensure the move doesn't uncover check.
    /// 
    ///  Therefore the board will be resposible for managing this behaviour. See <see cref="ChessBoard.MovesFor"/>
    ///  </remarks>
    ///  </summary>
    public abstract class MoveGeneratorBase : IMoveGenerator
    {
        public abstract IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at);
    }
}