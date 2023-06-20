using System;
using BoringOS.Time;
using Cosmos.HAL;
using Global = Cosmos.HAL.Global;

namespace BoringOS.Kernel.Time;

public class CPUKernelTimer : KernelTimer
{
    private const long OneSecondNs = 1_000_000_000;
    private const long OneSecondTs = 10_000;
    // private const uint Frequency = 1_000;
    private const uint Frequency = 10; // for some reason, this is the best precision i can get.
    private const long Precision = OneSecondNs / Frequency;
    
    private long _elapsedTicks;

    // protected override long Now => (long)(CPU.GetCPUUptime() / 1000);
    protected override long Now => this._elapsedTicks;

    public override void Start()
    {
        Global.PIT.RegisterTimer(new PIT.PITTimer(() =>
        {
            this._elapsedTicks += OneSecondTs * Frequency * 10;
        }, Precision, true));
    }
}