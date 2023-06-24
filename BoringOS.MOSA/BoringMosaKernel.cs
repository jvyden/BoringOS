using BoringOS.MOSA.Network;
using BoringOS.Network;
using BoringOS.Threading;
using Mosa.Kernel.x86;

namespace BoringOS.MOSA;

public class BoringMosaKernel : BoringKernel
{
    protected override bool NeedsManualGarbageCollection => false;
    public override long CollectGarbage()
    {
        return -1;
    }

    public override long GetUsedMemory()
    {
        return 0;
    }

    protected override void WriteAll(string message)
    {
        Screen.WriteLine(message);
    }

    protected override SystemInformation CollectSystemInfo()
    {
        return new SystemInformation
        {
            CPUVendor = "Mosa",
            CPUBrand = "Bogus Data",
            MemoryCountKilobytes = 0,
        };
    }

    protected override NetworkManager InstantiateNetworkManager()
    {
        return new MosaNetworkManager();
    }

    protected override ProcessManager InstantiateProcessManager()
    {
        return new FakeProcessManager();
    }
}