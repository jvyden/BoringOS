using System.Diagnostics.CodeAnalysis;
using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;

namespace BoringOS.MOSA;

public static class Boot
{
    [Plug("Mosa.Runtime.StartUp::SetInitialMemory")]
    public static void SetInitialMemory()
    {
        KernelMemory.SetInitialMemory(Address.GCInitialMemory, 0x01000000);
    }

    [DoesNotReturn]
    public static void Main()
    {
        Kernel.Setup();
        IDT.SetInterruptHandler(ProcessInterrupt);

        Program.Setup();

        for (;;)
            Program.Loop();
        // ReSharper disable once FunctionNeverReturns
    }

    public static void ProcessInterrupt(uint interrupt, uint errorCode)
    {
        if (interrupt >= 0x20 && interrupt < 0x30)
            HAL.ProcessInterrupt((byte)(interrupt - 0x20));
    }
}