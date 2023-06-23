using System;
using BoringOS.Kernel.Network;
using BoringOS.Kernel.Terminal;
using BoringOS.Kernel.Threading;
using BoringOS.Network;
using BoringOS.Terminal;
using BoringOS.Threading;
using BoringOS.Time;
using Cosmos.Core;
using Cosmos.Core.Memory;
using Cosmos.Core.Multiboot;
using Cosmos.HAL;

namespace BoringOS.Kernel;

public class BoringBareMetalKernel : AbstractBoringKernel
{
    protected override bool NeedsManualGarbageCollection => true;
#if VBE
    internal TerminalType TerminalType = TerminalType.Canvas;
#else
    internal TerminalType TerminalType = TerminalType.Console;
#endif

    protected override SystemInformation CollectSystemInfo()
    {
        Console.WriteLine("    Acquiring vendor name and RAM");
        SystemInformation info = new()
        {
            CPUVendor = CPU.GetCPUVendorName(),
            MemoryCountKilobytes = Multiboot2.GetMemLower() + Multiboot2.GetMemUpper(),
        };

        Console.Write("    Checking if we can read CPUID... ");
#if false
        bool canReadCpuId = CPU.CanReadCPUID() != 0;
#else
        bool canReadCpuId = false;
#endif
        Console.Write(canReadCpuId ? "Yes!" : "No.");
        Console.Write('\n');

        info.CPUBrand = canReadCpuId ? CPU.GetCPUBrandString() : "Unknown";

        return info;
    }

    public override long CollectGarbage()
    {
        long allocatedBefore = GCImplementation.GetUsedRAM();
        Heap.Collect();
        return allocatedBefore - GCImplementation.GetUsedRAM();
    }

    public override long GetUsedMemory()
    {
        return GCImplementation.GetUsedRAM();
    }

    protected override void WriteAll(string message)
    {
        SerialPort.SendString(message);
        Console.WriteLine(message);
    }

    protected override void PrintException(Exception e)
    {
        while (true)
        {
            Console.WriteLine($"{e.GetType().Name}: {e.Message}");
            if (e.InnerException != null)
            {
                e = e.InnerException;
                continue;
            }

            break;
        }
    }

    protected override ITerminal InstantiateTerminal() => TerminalType switch
    {
        TerminalType.Console => new ConsoleTerminal(),
        TerminalType.Serial => new SerialTerminal(),
        TerminalType.Canvas => new CanvasTerminal(),
        _ => throw new ArgumentOutOfRangeException()
    };

    protected override NetworkManager InstantiateNetworkManager() => new CosmosNetworkManager();
    protected override AbstractProcessManager InstantiateProcessManager() => new ZarloProcessManager();

    // Use UtcNowKernelTimer, uses RTC to avoid usage of PIT
    // TODO: Make a thread-based timer
    public override KernelTimer InstantiateTimer() => new UtcNowKernelTimer();
}