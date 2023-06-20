using System.Diagnostics;

namespace BoringOS.Extensions;

public static class StopwatchExtensions
{
    public static long ElapsedNanoseconds(this Stopwatch stopwatch)
    {
        long nanoseconds = (long)(stopwatch.ElapsedTicks * 1_000_000_000.0 / Stopwatch.Frequency);
        return nanoseconds;
    }
}