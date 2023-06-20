namespace BoringOS.Emulator;

public class BoringEmulatedKernel : AbstractBoringKernel
{
    public bool KernelIsRunning = true;
    protected override bool NeedsManualGarbageCollection => false;
    public override bool HaltKernel()
    {
        this.KernelIsRunning = false;
        return true;
    }

    public override int CollectGarbage()
    {
        GC.Collect();
        return -1;
    }

    public override void WriteAll(string message)
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