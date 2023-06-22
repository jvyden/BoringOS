using BoringOS.Emulator.Network;
using BoringOS.Network;

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

    public override long GetAllocatedMemory()
    {
        return GC.GetTotalMemory(false);
    }

    protected override void WriteAll(string message)
    {
        Console.WriteLine(message);
    }

    protected override SystemInformation CollectSystemInfo()
    {
        return new SystemInformation
        {
            MemoryCountKilobytes = 1024,
            CPUVendor = "EmuEmuEmuEmu",
            CPUBrand = "Emulated Kernel"
        };
    }

    protected override NetworkManager InstantiateNetworkManager() => new EmulatedNetworkManager();
}