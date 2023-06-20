using BoringOS.Programs;
using BoringOS.Terminal;
using BoringOS.Time;

namespace BoringOS;

public class BoringSession
{
    public BoringSession(ITerminal terminal, SystemInformation systemInformation, KernelTimer sysTimer, List<Program> programs, AbstractBoringKernel kernel)
    {
        this.Terminal = terminal;
        this.SystemInformation = systemInformation;
        this.Kernel = kernel;
        this.Programs = programs;
        this._sysTimer = sysTimer;
        
        this._sessionTimer = kernel.InstantiateTimer();
        this._sessionTimer.Start();

        this.SessionId = _sessionIncrement;
        _sessionIncrement++;
    }

    private static uint _sessionIncrement = 0;
    public uint SessionId { get; private init; }

    public readonly ITerminal Terminal;
    public readonly SystemInformation SystemInformation;
    public readonly AbstractBoringKernel Kernel;
    public readonly List<Program> Programs;
    
    private readonly KernelTimer _sysTimer;
    public long ElapsedKernelNanoseconds => this._sysTimer.ElapsedNanoseconds;
    public long ElapsedKernelMilliseconds => this._sysTimer.ElapsedMilliseconds;

    private readonly KernelTimer _sessionTimer;
    public long ElapsedSessionNanoseconds => this._sessionTimer.ElapsedNanoseconds;
    public long ElapsedSessionMilliseconds => this._sessionTimer.ElapsedMilliseconds;
}