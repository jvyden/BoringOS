using System;
using BoringOS.Kernel.Terminal;
using BoringOS.Terminal;
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
    
    public override bool HaltKernel()
    {
        CPU.Halt();
        return true;
    }

    public override int CollectGarbage()
    {
        return Heap.Collect();
    }

    protected override void WriteAll(string message)
    {
        SerialPort.SendString(message);
        Console.WriteLine(message);
    }

    protected override ITerminal InstantiateTerminal()
    {
        return new SerialTerminal();
    }
}