using System.Threading;

namespace BoringOS.Threading;

/// <summary>
/// A process manager that invokes tasks directly instead of using multitasking.
/// Basically, a single-threaded mode for the kernel.
/// </summary>
public class FakeProcessManager : ProcessManager
{
    public override void Initialize()
    {
        // No initialization required
    }

    public override void StartProcess(Action start)
    {
        start.Invoke();
    }

    public override void Sleep(int ms)
    {
        Thread.Sleep(ms);
    }
}