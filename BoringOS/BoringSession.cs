using BoringOS.Programs;
using BoringOS.Terminal;
using BoringOS.Time;

namespace BoringOS;

public class BoringSession
{
    public BoringSession(ITerminal terminal, List<Program> programs, BoringKernel kernel)
    {
        this.Terminal = terminal;
        this.Programs = programs;
        this.Kernel = kernel;

        this._sessionTimer = kernel.InstantiateTimer();
        this._sessionTimer.Start();

        this.SessionId = _sessionIncrement;
        _sessionIncrement++;
    }

    private static uint _sessionIncrement = 0;
    public uint SessionId { get; private set; }

    public readonly ITerminal Terminal;
    public readonly BoringKernel Kernel;
    public readonly List<Program> Programs;
    

    private readonly KernelTimer _sessionTimer;
    public long ElapsedMilliseconds => this._sessionTimer.ElapsedMilliseconds;
}