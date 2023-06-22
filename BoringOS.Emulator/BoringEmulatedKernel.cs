using BoringOS.Emulator.Network;
using BoringOS.Emulator.Threading;
using BoringOS.Network;
using BoringOS.Threading;

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

    public override long GetUsedMemory()
    {
        return GC.GetTotalMemory(false);
    }

    protected override void WriteAll(string message)
    {
        Console.WriteLine(message);
    }

    protected override SystemInformation CollectSystemInfo()
    {
        int memoryKilobytes = (int)(GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / 1_024);
        
        return new SystemInformation
        {
            MemoryCountKilobytes = (uint)memoryKilobytes,
            CPUVendor = "EmuEmuEmuEmu",
            CPUBrand = "Emulated Kernel"
        };
    }

    protected override NetworkManager InstantiateNetworkManager() => new EmulatedNetworkManager();
    protected override AbstractProcessManager InstantiateProcessManager() => new TaskProcessManager();
}