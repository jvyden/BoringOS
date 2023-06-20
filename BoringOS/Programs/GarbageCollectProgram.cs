namespace BoringOS.Programs;

public class GarbageCollectProgram : Program
{
    public GarbageCollectProgram() : base("gc", "Frees memory by invoking garbage collection")
    {
    }

    public override byte Invoke(string[] args, BoringSession session)
    {
        long freed = session.Kernel.CollectGarbage();
        session.Terminal.WriteString($"Freed {freed / 1048576}MB of memory\n");
        return 0;
    }
}