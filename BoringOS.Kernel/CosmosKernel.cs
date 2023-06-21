using System;
using System.Threading;
using Cosmos.System;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using JetBrains.Annotations;
using Console = System.Console;
using Global = Cosmos.System.Global;

namespace BoringOS.Kernel;

public class CosmosKernel : Cosmos.System.Kernel
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

        Thread.Sleep(25); // Sleep for a bit to wait for a key
        if (KeyboardManager.TryReadKey(out KeyEvent key) && key.KeyChar == 's')
        {
            Console.WriteLine("Telling kernel to instantiate a serial terminal");
            this._kernel.UseSerial = true;
        }
        
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