namespace board.engine
{
    public static class FeatureFlags
    {
        // NOTE: 20/05/2019 - This feature seems ok, leaving flag in for now,
        // easier to debug sequentially sometimes
        public static bool ParalleliseRefreshAllPaths = true;


        // NOTE: 20/05/2019 - This one is causing a race condition 
        // something around checking for enemy attacks, in the clone maybe
        // Can this be safely parallelised?
        //  * Move validation can depend on the moves of enemy pieces
        //    which are being generated in parallel
        //  POSSIBLE DIFFEERENT APPROACH TO CHECKING A SQUARE IS UNDER ATTACK
        //     From the square do 4path checks in all directions (plus Knight moves) for enemy pieces/
        public static bool ParalleliseRemoveInvalidMoves = false;


        // Need proper boardstate key I think 
        public static bool CachingPaths = false;

    }
}