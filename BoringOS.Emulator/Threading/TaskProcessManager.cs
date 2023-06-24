using BoringOS.Threading;

namespace BoringOS.Emulator.Threading;

public class TaskProcessManager : ProcessManager
{
    public override void Initialize()
    {
        // No initialization required
    }

    public override void StartProcess(Action start)
    {
        Task.Factory.StartNew(start);
    }

    public override void Sleep(int ms)
    {
        Thread.Sleep(ms);
    }
}