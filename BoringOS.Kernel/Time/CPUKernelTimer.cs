using System;
using BoringOS.Time;
using Cosmos.HAL;
using Global = Cosmos.HAL.Global;

namespace BoringOS.Kernel.Time;

public class CPUKernelTimer : KernelTimer
{
    private long _elapsedTicks;

    // protected override long Now => (long)(CPU.GetCPUUptime() / 1000);
    protected override long Now => this._elapsedTicks;

    public override void Start()
    {
        const long precision = 1_000_000_000 / 10; // 1 second divided by n 
        this.StartTicks = _elapsedTicks = DateTime.UtcNow.Ticks;
        Global.PIT.RegisterTimer(new PIT.PITTimer(() =>
        {
            this._elapsedTicks += precision / 100;
        }, precision, true));
    }
}