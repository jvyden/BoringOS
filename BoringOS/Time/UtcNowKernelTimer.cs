namespace BoringOS.Time;

public class UtcNowKernelTimer : KernelTimer
{
    protected override long Now => DateTime.UtcNow.Ticks;
}