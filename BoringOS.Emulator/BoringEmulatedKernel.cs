namespace BoringOS.Emulator;

public class BoringEmulatedKernel : AbstractBoringKernel
{
    protected override bool NeedsManualGarbageCollection => false;

    public override long CollectGarbage()
    {
        long allocatedBefore = GC.GetTotalMemory(false);
        GC.Collect();
        return allocatedBefore - GC.GetTotalMemory(false);
    }

    protected override void WriteAll(string message)
    {
        Console.WriteLine(message);
    }

    protected override SystemInformation GetSystemInformation()
    {
        return new SystemInformation
        {
            MemoryCountMegabytes = 1,
            EstimatedCycleSpeed = -1,
            CPUVendor = "EmuEmuEmuEmu",
            CPUBrand = "Emulated Kernel"
        };
    }
}