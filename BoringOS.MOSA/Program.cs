using System;
using JetBrains.Annotations;

namespace BoringOS.MOSA;

[UsedImplicitly]
public static class Program
{
    private static BoringMosaKernel _kernel = null!;
    
    public static void Setup()
    {
        Console.WriteLine("Setup complete, jumping to BoringMosaKernel");
        _kernel = new BoringMosaKernel();
        _kernel.OnBoot();
        _kernel.BeforeRun();
    }

    public static void Loop()
    {
        _kernel.Run();
    }
}