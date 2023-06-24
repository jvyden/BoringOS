using System;
using BoringOS.Cosmos.Terminal;
using Cosmos.Debug.Kernel;
using Cosmos.System;
using JetBrains.Annotations;
using Console = System.Console;
using Global = Cosmos.System.Global;

namespace BoringOS.Cosmos;

public class CosmosKernel : Kernel
{
    private BoringBareMetalKernel _kernel = null!;

    protected override void OnBoot()
    {
        DebuggerFactory.WriteToConsole = true;
        Global.Init(this.GetTextScreen(), false, true, false, false);

        this._kernel = new BoringBareMetalKernel();
        this._kernel.OnBoot();
    }

    private void HandleStartupKey(char c)
    {
        switch (c)
        {
            case 'c':
#if VBE
                Console.WriteLine("!! Will boot into console terminal !!");
                this._kernel.TerminalType = TerminalType.Console;
#else
                Console.WriteLine("!! Will boot into canvas terminal !!");
                this._kernel.TerminalType = TerminalType.Canvas;
#endif
                break;
            default:
                Console.WriteLine($"!!! Unknown startup key {c} !!!");
                break;
        }
    }

    [UsedImplicitly]
    protected override void BeforeRun()
    {
        Console.WriteLine("Cosmos kernel initialized, jumping to BoringKernel");
        Console.ForegroundColor = ConsoleColor.Gray;
        
        // Wait for key without PIT to avoid breaking multi-threading
        // Very stupid but it works
        // TODO: Replace with noop?
        for (int i = 0; i < 10_000; i++)
        {
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }

        if (KeyboardManager.TryReadKey(out KeyEvent key)) 
            this.HandleStartupKey(key.KeyChar);

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