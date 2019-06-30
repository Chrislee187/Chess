namespace board.engine
{
    public static class FeatureFlags
    {
        // NOTE: 20/05/2019 - This feature seems ok, leaving flag in for now,
        // easier to debug sequentially sometimes
        public static bool ParalleliseRefreshAllPaths = true;


        // NOTE: 21/05/2019 - Pretty sure we can't do this without risking race conditions
        // 
        public static bool ParalleliseRemoveInvalidMoves = false;


        // Need proper boardstate key I think 
        public static bool CachingPaths = false;

    }
}