using System;
using Cosmos.System;
using JetBrains.Annotations;
using Console = System.Console;

namespace BoringOS;

public class BoringKernel : Kernel
{
    private BoringShell _shell = null!;

    protected override void OnBoot()
    {
        Global.Init(this.GetTextScreen(), false, true, false, false);
    }

    [UsedImplicitly]
    protected override void BeforeRun()
    {
        Console.WriteLine("Cosmos kernel initialized");

        // Console.Clear();
        Console.WriteLine($"\nWelcome to BoringOS {BoringVersionInformation.Type} (commit {BoringVersionInformation.CommitHash})");
        
        // Setup
        this._shell = new BoringShell();
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
            Console.Clear();
            Console.WriteLine(e);
        }
    }
}