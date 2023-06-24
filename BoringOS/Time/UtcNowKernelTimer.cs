namespace BoringOS.Time;

public class UtcNowKernelTimer : KernelTimer
{
#if !DEBUGMOSA
    protected override long Now => DateTime.UtcNow.Ticks;
#else
    protected override long Now => 0;
#endif
}