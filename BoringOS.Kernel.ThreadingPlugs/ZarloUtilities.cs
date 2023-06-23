using Zarlo.XSharp;

namespace BoringOS.Kernel.ThreadingPlugs;

public static class ZarloUtilities
{
    public static void CallSwitchTask() => throw new ImplementedInPlugException();
}