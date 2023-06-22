using System;
using System.Threading;
using BoringOS.Kernel.Terminal;
using Cosmos.System;
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

        // Thread.Sleep(25); // Sleep for a bit to wait for a key
        // if (KeyboardManager.TryReadKey(out KeyEvent key)) 
            // HandleStartupKey(key.KeyChar);

        if (KeyboardManager.ShiftPressed)
        {
            char key = Console.ReadKey(true).KeyChar;
            HandleStartupKey(key);
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