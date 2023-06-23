using IL2CPU.API;
using IL2CPU.API.Attribs;
using XSharp;
using XSharp.Assembler;
using Zarlo.Cosmos.Threading;
using Zarlo.Cosmos.Threading.Core.Processing;

// ReSharper disable all

namespace BoringOS.Kernel.ThreadingPlugs;

[Plug(Target = typeof(ObjUtilities))]
public static unsafe class ObjUtilitiesImpl
{
    [PlugMethod(Assembler = typeof(ObjUtilitiesGetPointer))]
    public static uint GetPointer(Delegate aVal) { return 0; }

    [PlugMethod(Assembler = typeof(ObjUtilitiesGetPointer))]
    public static uint GetPointer(Object aVal) { return 0; }

    [PlugMethod(Assembler = typeof(ObjUtilitiesGetEntry))]
    public static uint GetEntryPoint() { return 0; }
}

public class ObjUtilitiesGetPointer : AssemblerMethod
{
    public override void AssembleNew(Assembler aAssembler, object aMethodInfo)
    {
        XS.Set(XSRegisters.EAX, XSRegisters.EBP, sourceDisplacement: 0x8);
        XS.Push(XSRegisters.EAX);
    }
}

public class ObjUtilitiesGetEntry : AssemblerMethod
{
    public override void AssembleNew(Assembler aAssembler, object aMethodInfo)
    {
        XS.Set(XSRegisters.EAX, LabelName.Get(Zarlo.XSharp.Utils.GetMethodDef(
                typeof(ProcessorScheduler),
                nameof(ProcessorScheduler.EntryPoint),
                true
                )
            )
        );
        XS.Push(XSRegisters.EAX);
    }
}