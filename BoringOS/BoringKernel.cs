using System;
using CosmosKernel = Cosmos.System.Kernel;

namespace BoringOS;

public class Kernel : CosmosKernel
{
    protected override void BeforeRun()
    {
        Console.WriteLine("Cosmos booted successfully. Type a line of text to get it echoed back.");
    }

    protected override void Run()
    {
        Console.Write("Input: ");
        var input = Console.ReadLine();
        Console.Write("Text typed: ");
        Console.WriteLine(input);
    }
}