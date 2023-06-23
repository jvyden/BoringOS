using Cosmos.Core;
using IL2CPU.API.Attribs;
using Zarlo.XSharp;

namespace BoringOS.Kernel.ThreadingPlugs;

// ReSharper disable all

[Plug(Target = typeof(CPU), IsOptional = false)]
public class CPUImpl
{
    [PlugMethod(Assembler = typeof(CPUUpdateIDTAsm), IsOptional = false)]
    public static void UpdateIDT(bool aEnableInterruptsImmediately) => throw new ImplementedInPlugException();
}