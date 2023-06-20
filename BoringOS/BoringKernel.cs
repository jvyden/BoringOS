using Cosmos.System;
using JetBrains.Annotations;
using Console = System.Console;

namespace BoringOS;

public class BoringKernel : Kernel
{
    [UsedImplicitly]
    protected override void BeforeRun()
    {
        Console.WriteLine("Cosmos started");
        Console.WriteLine($"BoringOS {BoringVersionInformation.Type} {BoringVersionInformation.CommitHash}");
    }

    [UsedImplicitly]
    protected override void Run()
    {
        Console.Write("Input: ");
        string input = Console.ReadLine();
        Console.Write("Text typed: ");
        Console.WriteLine(input);
    }
}