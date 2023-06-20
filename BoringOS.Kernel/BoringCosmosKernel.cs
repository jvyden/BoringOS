using System;
using Cosmos.Core.Memory;
using JetBrains.Annotations;
using Global = Cosmos.System.Global;

namespace BoringOS.Kernel;

public class BoringCosmosKernel : Cosmos.System.Kernel
{
    private BoringBareMetalKernel _kernel = null!;

    protected override void OnBoot()
    {
        _kernel = new BoringBareMetalKernel();
        this._kernel.OnBoot();

        Global.Init(this.GetTextScreen(), false, true, false, false);
    }

    [UsedImplicitly]
    protected override void BeforeRun()
    {
        Console.WriteLine("Cosmos kernel initialized, jumping to BoringBareMetalKernel");
        Console.ForegroundColor = ConsoleColor.Gray;
        this._kernel.BeforeRun();
    }

    [UsedImplicitly]
    protected override void Run()
    {
        this._kernel.Run();
    }

    protected override void AfterRun()
    {
        this._kernel.AfterRun();
    }
}