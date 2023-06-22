using BoringOS.Time;

namespace BoringOS.Programs;

public class GarbageCollectProgram : Program
{
    public GarbageCollectProgram() : base("gc", "Frees memory by invoking garbage collection")
    {
    }

    public override byte Invoke(string[] args, BoringSession session)
    {
        using KernelTimer timer = session.Kernel.InstantiateTimer();
        timer.Start();
        
        long freed = session.Kernel.CollectGarbage();
        long ms = timer.ElapsedMilliseconds;
        
        session.Terminal.WriteString($"Freed {freed / 1024}KB of memory\n");
        session.Terminal.WriteString($"Took {ms}ms\n");
        return 0;
    }
}