using System;
using BoringOS.Kernel.Terminal;
using BoringOS.Kernel.Time;
using BoringOS.Terminal;
using BoringOS.Time;
using Cosmos.Core;
using Cosmos.Core.Memory;
using Cosmos.HAL;

namespace BoringOS.Kernel;

public class BoringBareMetalKernel : AbstractBoringKernel
{
    protected override bool NeedsManualGarbageCollection => true;

    protected override SystemInformation GetSystemInformation()
    {
        Console.WriteLine("    Acquiring vendor name and RAM");
        SystemInformation info = new()
        {
            CPUVendor = CPU.GetCPUVendorName(),
            MemoryCountMegabytes = CPU.GetAmountOfRAM(),
        };
        
        Console.Write("    Checking if we can read CPUID... ");
#if false
        bool canReadCpuId = CPU.CanReadCPUID() != 0;
#else
        bool canReadCpuId = false;
#endif
        Console.Write(canReadCpuId ? "Yes!" : "No.");
        Console.Write('\n');

        if (canReadCpuId)
        {
            info.CPUBrand = CPU.GetCPUBrandString();
            info.EstimatedCycleSpeed = CPU.GetCPUCycleSpeed();
        }
        else
        {
            info.CPUBrand = "Unknown";
            info.EstimatedCycleSpeed = -1;
        }

        return info;
    }

    public override long CollectGarbage()
    {
        long allocatedBefore = GCImplementation.GetUsedRAM();
        Heap.Collect();
        return allocatedBefore - GCImplementation.GetUsedRAM();
    }

    protected override void WriteAll(string message)
    {
        SerialPort.SendString(message);
        Console.WriteLine(message);
    }

    // protected override ITerminal InstantiateTerminal() => new SerialTerminal();
    public override KernelTimer InstantiateTimer() => new CPUKernelTimer();
}