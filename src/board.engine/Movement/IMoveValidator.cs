namespace board.engine.Movement
{
    public interface IMoveValidator<TWrapper>
    {
        // TODO: Not happy with TWrapper approach, allows testing without setting a full board state
        // but is quiet clumsy to work with, more thought needed.
        // ISSUES:
        //  Want a standard ValidateMove() method to sit on an interface but
        //    Different implementations have different boardstate access requirements
        //      and want to reuse base level validations
        //    So...
        //      Base class with basic IsEmpty, ContainsFriend, ContainsEnemy, checks
        //      Then create ActionValidators for DefaultActions
        //      ComplexValidators - Require more than the basic IsEmpty... BoardState access
        //              but still want an generalised interface for the provider approach
        //                  but want an easy interface to test against.
        //          
        // POSSIBLE SOLUTION:
        //  IReadOnlyBoardState<> interface, would still allow access to change entities state indirectly
        //     but wouldn't allow any change to board locations (add/remove pieces)
        bool ValidateMove(BoardMove move, TWrapper wrapper);
    }
}