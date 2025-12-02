namespace ChargerAstronomyShared.Contracts.Streaming
{

    /// <summary>
    /// Engine statistics.
    /// </summary>
    public class EngineStats
    {
        public int TotalTiles { get; set; }
        public int ActiveTiles { get; set; }
        public int TotalStars { get; set; }
        public int ActiveStars { get; set; }
        public int ActivationQueueSize { get; set; }
        public int DeactivationQueueSize { get; set; }
        public int UpdateQueueSize { get; set; }
        public float CurrentMagnitudeCutoff { get; set; }

        public override string ToString()
        {
            return $"Tiles: {ActiveTiles}/{TotalTiles}, Stars: {ActiveStars}/{TotalStars} (mag<={CurrentMagnitudeCutoff:F1}), " +
                   $"Queues: A={ActivationQueueSize} D={DeactivationQueueSize} U={UpdateQueueSize}";
        }
    }
}