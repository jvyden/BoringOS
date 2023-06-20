using System.Diagnostics;
using BoringOS.Extensions;
using BoringOS.Terminal;

namespace BoringOS;

public class BoringSession
{
    public BoringSession(ITerminal terminal, SystemInformation systemInformation, Stopwatch sysTimer)
    {
        this.Terminal = terminal;
        this.SystemInformation = systemInformation;
        this._sysTimer = sysTimer;
        
        this._sessionTimer = new Stopwatch();
        this._sessionTimer.Start();

        this.SessionId = _sessionIncrement;
        _sessionIncrement++;
    }

    private static uint _sessionIncrement = 0;
    public uint SessionId { get; private init; }

    public readonly ITerminal Terminal;
    public readonly SystemInformation SystemInformation;
    
    private readonly Stopwatch _sysTimer;
    public long ElapsedKernelNanoseconds => this._sysTimer.ElapsedNanoseconds();
    public long ElapsedKernelMilliseconds => this._sysTimer.ElapsedMilliseconds;

    private readonly Stopwatch _sessionTimer;
    public long ElapsedSessionNanoseconds => this._sessionTimer.ElapsedNanoseconds();
    public long ElapsedSessionMilliseconds => this._sessionTimer.ElapsedMilliseconds;
}