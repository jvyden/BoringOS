using System;
using JetBrains.Annotations;
using Global = Cosmos.System.Global;

namespace BoringOS.Kernel;

public class BoringCosmosKernel : Cosmos.System.Kernel
{
    private BoringBareMetalKernel _kernel = null!;

    protected override void OnBoot()
    {
        Global.Init(this.GetTextScreen(), false, true, false, false);
        
        _kernel = new BoringBareMetalKernel();
        this._kernel.OnBoot();
    }

    [UsedImplicitly]
    protected override void BeforeRun()
    {
        Console.WriteLine("Cosmos kernel initialized, jumping to BoringKernel");
        Console.ForegroundColor = ConsoleColor.Gray;
        this._kernel.BeforeRun();
    }

    [UsedImplicitly]
    protected override void Run()
    {
        if(!this._kernel.KernelIsRunning) this.Stop();
        else this._kernel.Run();
    }

    protected override void AfterRun()
    {
        this._kernel.AfterRun();
    }
}