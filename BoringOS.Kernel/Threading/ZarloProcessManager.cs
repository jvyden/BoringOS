using System;
using System.Diagnostics.CodeAnalysis;
using BoringOS.Kernel.ThreadingPlugs;
using BoringOS.Threading;
using Cosmos.HAL;
using Zarlo.Cosmos.Threading;
using Zarlo.Cosmos.Threading.Core.Context;
using Zarlo.Cosmos.Threading.Core.Processing;

namespace BoringOS.Kernel.Threading;

public class ZarloProcessManager : AbstractProcessManager
{
    [SuppressMessage("ReSharper", "ConditionalAccessQualifierIsNonNullableAccordingToAPIContract")]
    public override void Initialize()
    {
        // ProcessorScheduler.SwitchTask();
        Global.PIT.RegisterTimer(new PIT.PITTimer(ZarloUtilities.CallSwitchTask, 0, true));
        
        ProcessorScheduler.Initialize();
        // ProcessorScheduler.EntryPoint();
    }

    public override void StartProcess(Action start)
    {
        Process process = new Process(new ThreadStart(start));
        process.Start();
    }

    public override void Sleep(int ms)
    {
        Thread.Sleep(ms);
    }
}