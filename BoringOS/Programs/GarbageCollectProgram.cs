using Cosmos.Core.Memory;

namespace BoringOS.Programs;

public class GarbageCollectProgram : Program
{
    public GarbageCollectProgram() : base("gc", "Frees memory by invoking garbage collection")
    {
    }

    public override byte Invoke(string[] args, BoringSession session)
    {
        int freed = Heap.Collect();
        session.Terminal.WriteString($"Freed {freed} objects");
        return 0;
    }
}