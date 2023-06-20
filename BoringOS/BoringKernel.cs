using System;
using System.Diagnostics;
using BoringOS.Extensions;
using BoringOS.Terminal;
using Cosmos.Core;
using Cosmos.HAL;
using Cosmos.System;
using JetBrains.Annotations;
using Console = System.Console;
using Global = Cosmos.System.Global;

namespace BoringOS;

public class BoringKernel : Kernel
{
    private ITerminal _terminal = null!;
    private BoringShell _shell = null!;

    private SystemInformation _information;
    
    private Stopwatch _sysTimer = null!;

    protected override void OnBoot()
    {
        this._sysTimer = new Stopwatch();
        this._sysTimer.Start();
        Global.Init(this.GetTextScreen(), false, true, false, false);
    }

    [UsedImplicitly]
    protected override void BeforeRun()
    {
        Console.WriteLine("Cosmos kernel initialized, jumping to BoringKernel");
        Console.ForegroundColor = ConsoleColor.Gray;
        
        Console.WriteLine("  Initializing terminal");
        // Set up terminal
        this._terminal = new ConsoleTerminal();
        // this._terminal = new SerialTerminal();

        Console.WriteLine("  Gathering SystemInformation");
        this._information = GetSystemInformation();

        Console.Write($"    CPU: {this._information.CPUVendor} {this._information.CPUBrand}, ");
        Console.WriteLine($"{this._information.MemoryCountMegabytes}MB of upper memory");

        this._terminal.WriteString($"\nWelcome to BoringOS {BoringVersionInformation.Type} (commit {BoringVersionInformation.CommitHash})\n");
        this._terminal.WriteString($"  Boot took {this._sysTimer.ElapsedNanoseconds()}ns\n");

        this._shell = new BoringShell(_terminal);
    }

    private static SystemInformation GetSystemInformation()
    {
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

    [UsedImplicitly]
    protected override void Run()
    {
        try
        {
            this._shell.TakeInput();
        }
        catch(Exception e)
        {
            SerialPort.SendString(e.ToString());
            
            Console.Clear();
            Console.WriteLine(e);
        }
    }
}