using System;
using System.Diagnostics.CodeAnalysis;
using BoringOS.Kernel.ThreadingPlugs;
using BoringOS.Threading;
using Zarlo.Cosmos.Threading;
using Zarlo.Cosmos.Threading.Core.Processing;

namespace BoringOS.Kernel.Threading;

public class ZarloProcessManager : ProcessManager
{
    [SuppressMessage("ReSharper", "ConditionalAccessQualifierIsNonNullableAccordingToAPIContract")]
    public override void Initialize()
    {
        BoringProcessScheduler.SwitchTask();
        ProcessorScheduler.Initialize();
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