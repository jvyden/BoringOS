using System;
using BoringOS.Threading;
using Zarlo.Cosmos.Threading;
using Zarlo.Cosmos.Threading.Core.Processing;

namespace BoringOS.Kernel.Threading;

public class ZarloProcessManager : AbstractProcessManager
{
    public override void Initialize()
    {
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