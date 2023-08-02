using System;
using System.Diagnostics.CodeAnalysis;
using BoringOS.MOSA.Hardware;
using BoringOS.MOSA.Network;
using BoringOS.MOSA.Terminal;
using BoringOS.Network;
using BoringOS.Terminal;
using BoringOS.Threading;
using Mosa.Kernel.x86;
using Mosa.Runtime.x86;

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
    
    protected override void PrintException(Exception e)
    {
        while (true)
        {
            this.KernelTerminal.WriteString(e.ToString());
            this.KernelTerminal.WriteChar('\n');
            Panic.DumpStackTrace();
            if (e.InnerException != null)
            {
                e = e.InnerException;
                continue;
            }

            break;
        }
    }

    [DoesNotReturn]
    public override bool Halt()
    {
        while (true)
        {
            Native.Hlt();
        }
        // ReSharper disable once FunctionNeverReturns
    }

    protected override ITerminal InstantiateKernelTerminal()
    {
        return new ScreenTerminal(InputManager.Keyboards[0]);
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