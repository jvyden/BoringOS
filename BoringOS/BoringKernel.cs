using System;
using BoringOS.Terminal;
using Cosmos.HAL;
using Cosmos.System;
using JetBrains.Annotations;
using Console = System.Console;
using Global = Cosmos.System.Global;

namespace BoringOS;

public class BoringKernel : Kernel
{
    private ITerminal _terminal = null!;
    private BoringShell _shell = null!;

    protected override void OnBoot()
    {
        Global.Init(this.GetTextScreen(), false, true, false, false);
    }

    [UsedImplicitly]
    protected override void BeforeRun()
    {
        Console.WriteLine("Cosmos kernel initialized, jumping to BoringKernel");
        
        // Set up terminal
        this._terminal = new ConsoleTerminal();
        // this._terminal = new SerialTerminal();
        
        this._terminal.WriteString($"\nWelcome to BoringOS {BoringVersionInformation.Type} (commit {BoringVersionInformation.CommitHash})\n");

        this._shell = new BoringShell(_terminal);
    }

    [UsedImplicitly]
    protected override void Run()
    {
        try
        {
            this._shell.TakeInput();
        }
        catch(Exception e)
        {
            SerialPort.SendString(e.ToString());
            
            Console.Clear();
            Console.WriteLine(e);
        }
    }
}