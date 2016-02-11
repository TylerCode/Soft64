namespace Soft64.RCP
{
    public class ParallelInterfaceMemory
    {
        public uint Domain1Latency { get; internal set; }
        public uint Domain1PageSize { get; internal set; }
        public uint Domain1PulseWidth { get; internal set; }
        public uint Domain1Release { get; internal set; }
        public int Status { get; internal set; }
    }
}