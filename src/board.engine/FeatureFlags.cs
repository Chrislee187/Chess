namespace board.engine
{
    public static class FeatureFlags
    {
        // NOTE: 20/05/2019 - Using these flags to easily control parallelism when debugging
        public static bool ParalleliseRefreshAllPaths = true;


        // NOTE: This one is causing a race condition when generating the clone and checking the new move
        public static bool ParalleliseRemoveInvalidMoves = false;
    }
}