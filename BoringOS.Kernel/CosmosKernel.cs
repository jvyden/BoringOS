using System;
using BoringOS.Kernel.Terminal;
using Cosmos.System;
using JetBrains.Annotations;
using Zarlo.Cosmos.Threading;
using Zarlo.Cosmos.Threading.Core.Processing;
using Console = System.Console;
using Global = Cosmos.System.Global;
using SystemThread = System.Threading.Thread;

namespace BoringOS.Kernel;

public class CosmosKernel : Cosmos.System.Kernel
{
    private BoringBareMetalKernel _kernel = null!;

    protected override void OnBoot()
    {
        Global.Init(this.GetTextScreen(), false, true, false, false);
        
        ProcessorScheduler.Initialize();

        _kernel = new BoringBareMetalKernel();
        this._kernel.OnBoot();
    }

    private void HandleStartupKey(char c)
    {
        switch (c)
        {
            case 's':
                Console.WriteLine("!! Will boot into serial terminal !!");
                this._kernel.TerminalType = TerminalType.Serial;
                break;
            case 'c':
                Console.WriteLine("!! Will boot into canvas terminal !!");
                this._kernel.TerminalType = TerminalType.Canvas;
                break;
        }
    }

    [UsedImplicitly]
    protected override void BeforeRun()
    {
        Console.WriteLine("Cosmos kernel initialized, jumping to BoringKernel");
        Console.ForegroundColor = ConsoleColor.Gray;

        Thread.Sleep(25); // Sleep for a bit to wait for a key
        if (KeyboardManager.TryReadKey(out KeyEvent key)) 
            HandleStartupKey(key.KeyChar);

        this._kernel.BeforeRun();
        
        Console.WriteLine("!! Starting test thread !!");
        Process process = new(() =>
        {
            int i = 0;
            while (true)
            {
                Console.WriteLine("Thread tick " + i++);
                Thread.Sleep(1000);
            }
        });
                
        process.Start();
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