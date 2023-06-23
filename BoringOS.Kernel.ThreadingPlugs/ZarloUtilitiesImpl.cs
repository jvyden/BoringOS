using System.Reflection;
using IL2CPU.API;
using IL2CPU.API.Attribs;
using XSharp;
using XSharp.Assembler;
using Zarlo.Cosmos.Threading.Core.Processing;
using Zarlo.XSharp;
using static XSharp.XSRegisters;

namespace BoringOS.Kernel.ThreadingPlugs;

// ReSharper disable all

[Plug(Target = typeof(ZarloUtilities))]
public static unsafe class ZarloUtilitiesImpl
{
    [PlugMethod(Assembler = typeof(ZarloUtilitiesCallSwitchTask))]
    public static void CallSwitchTask() => throw new ImplementedInPlugException();
}

public class ZarloUtilitiesCallSwitchTask : AssemblerMethod
{
    public override void AssembleNew(Assembler aAssembler, object aMethodInfo)
    {
        string? stackContext = LabelName.GetStaticFieldName(typeof(Zarlo.Cosmos.Core.ZINTs), nameof(Zarlo.Cosmos.Core.ZINTs.mStackContext));
        MethodBase switchTaskMethod = Utils.GetMethodDef(
            typeof(ProcessorScheduler),
            nameof(ProcessorScheduler.SwitchTask)
        );
        
        string? switchTask = LabelName.Get(switchTaskMethod);
        
        // Backup registers onto stack
        _ = new LiteralAssemblerCode("pushad");
        _ = new LiteralAssemblerCode("mov eax, ds");
        _ = new LiteralAssemblerCode("push eax");
        _ = new LiteralAssemblerCode("mov eax, es");
        _ = new LiteralAssemblerCode("push eax");
        _ = new LiteralAssemblerCode("mov eax, fs");
        _ = new LiteralAssemblerCode("push eax");
        _ = new LiteralAssemblerCode("mov eax, gs");
        _ = new LiteralAssemblerCode("push eax");
        
        // Set all registers to 0x10 (?)
        _ = new LiteralAssemblerCode("mov ax, 0x10");
        
        _ = new LiteralAssemblerCode("mov ds, ax");
        _ = new LiteralAssemblerCode("mov es, ax");
        _ = new LiteralAssemblerCode("mov fs, ax");
        _ = new LiteralAssemblerCode("mov gs, ax");

        _ = new LiteralAssemblerCode("mov eax, esp"); // Move esp (stack pointer) to eax
        
        XS.Set(stackContext, EAX, destinationIsIndirect: true);
        XS.Call(switchTask);
        XS.Set(EAX, stackContext, sourceIsIndirect: true);
        
        // Restore registers from stack
        _ = new LiteralAssemblerCode("mov esp, eax");
        _ = new LiteralAssemblerCode("pop eax");
        _ = new LiteralAssemblerCode("mov gs, eax");
        _ = new LiteralAssemblerCode("pop eax");
        _ = new LiteralAssemblerCode("mov fs, eax");
        _ = new LiteralAssemblerCode("pop eax");
        _ = new LiteralAssemblerCode("mov es, eax");
        _ = new LiteralAssemblerCode("pop eax");
        _ = new LiteralAssemblerCode("mov ds, eax");
        _ = new LiteralAssemblerCode("popad");
    }
}