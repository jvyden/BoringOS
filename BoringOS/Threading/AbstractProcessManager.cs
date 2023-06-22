namespace BoringOS.Threading;

public abstract class AbstractProcessManager
{
    public abstract void Initialize();
    public abstract void StartProcess(Action start);
    public abstract void Sleep(int ms);
}