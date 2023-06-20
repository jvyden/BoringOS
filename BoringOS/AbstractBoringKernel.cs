using System.Diagnostics;
using BoringOS.Extensions;
using BoringOS.Programs;
using BoringOS.Terminal;

namespace BoringOS;

public abstract class AbstractBoringKernel
{
    private ITerminal _terminal = null!;
    private BoringShell _shell = null!;
    private BoringSession _session = null!;
    private SystemInformation _information;
    
    private Stopwatch _sysTimer = null!;
    
    protected abstract bool NeedsManualGarbageCollection { get; }
    
    public abstract bool HaltKernel();
    public abstract int CollectGarbage();
    public abstract void WriteAll(string message);
    protected abstract SystemInformation GetSystemInformation();

    public void OnBoot()
    {
        this._sysTimer = new Stopwatch();
        this._sysTimer.Start();
    }

    public void BeforeRun()
    {
        Console.WriteLine("  Initializing terminal");
        // Set up terminal
        this._terminal = new ConsoleTerminal();
        // this._terminal = new SerialTerminal();

        Console.WriteLine("  Gathering SystemInformation");
        this._information = GetSystemInformation();

        Console.Write($"    CPU: {this._information.CPUVendor} {this._information.CPUBrand}, ");
        Console.WriteLine($"{this._information.MemoryCountMegabytes}MB of upper memory");

        if (this.NeedsManualGarbageCollection)
        {
            int freed = this.CollectGarbage();
            Console.WriteLine($"  Freed {freed} objects");
        }

        this._terminal.WriteString($"\nWelcome to BoringOS {BoringVersionInformation.Type} (commit {BoringVersionInformation.CommitHash})\n");
        this._terminal.WriteString($"  Boot took {this._sysTimer.ElapsedNanoseconds()}ns\n");

        List<Program> programs = new()
        {
            new EchoProgram(),
            new GarbageCollectProgram(),
            new HaltProgram(),
            new HelpProgram(),
        };

        this._session = new BoringSession(this._terminal, this._information, this._sysTimer, programs, this);
        this._shell = new BoringShell(this._session);
    }

    public void Run()
    {
        try
        {
            this._shell.InputCycle();
        }
        catch(Exception e)
        {
            this.WriteAll(e.ToString());
        }

        if(this.NeedsManualGarbageCollection) 
            this.CollectGarbage();
    }

    public void AfterRun()
    {
        this.WriteAll("\n\nThe kernel has stopped. Halt.");
        this.HaltKernel();
    }
}