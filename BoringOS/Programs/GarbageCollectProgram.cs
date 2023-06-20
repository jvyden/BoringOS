namespace BoringOS.Programs;

public class GarbageCollectProgram : Program
{
    public GarbageCollectProgram() : base("gc", "Frees memory by invoking garbage collection")
    {
    }

    public override byte Invoke(string[] args, BoringSession session)
    {
        int freed = session.Kernel.CollectGarbage();
        session.Terminal.WriteString($"Freed {freed} objects");
        return 0;
    }
}