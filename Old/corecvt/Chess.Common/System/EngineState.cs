namespace Chess.Common.System
{
    public enum EngineState
    {
        Initialising,
        Waiting,
        GeneratingMoveLists,
        Moving,
        Started,
        GeneratingPieceMoves
    }
}