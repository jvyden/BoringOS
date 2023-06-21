using BoringOS.Programs;
using BoringOS.Terminal;
using BoringOS.Time;

namespace BoringOS;

public abstract partial class AbstractBoringKernel
{
    private ITerminal _terminal = null!;
    private BoringShell _shell = null!;
    private BoringSession _session = null!;
    private SystemInformation _information;
    
    private KernelTimer _sysTimer = null!;
    
    protected abstract bool NeedsManualGarbageCollection { get; }
    
    public abstract long CollectGarbage();
    public abstract long GetAllocatedMemory();
    protected abstract void WriteAll(string message);
    protected abstract SystemInformation GetSystemInformation();

    protected virtual ITerminal InstantiateTerminal() => new ConsoleTerminal();
    public virtual KernelTimer InstantiateTimer() => new UtcNowKernelTimer();

    private partial List<Program> InstantiatePrograms();

    public void OnBoot()
    {
        this._sysTimer = this.InstantiateTimer();
        this._sysTimer.Start();
    }

    public void BeforeRun()
    {
        Console.WriteLine("  Initializing terminal");
        
        // Set up terminal
        this._terminal = this.InstantiateTerminal();

        Console.WriteLine("  Gathering SystemInformation");
        this._information = GetSystemInformation();

        Console.Write($"    CPU: {this._information.CPUVendor} {this._information.CPUBrand}, ");
        Console.WriteLine($"{this._information.MemoryCountMegabytes}MB of upper memory");

        if (this.NeedsManualGarbageCollection)
        {
            long freed = this.CollectGarbage();
            Console.WriteLine($"  Freed {freed / 1048576}MB of memory");
        }

        this._terminal.WriteString($"\nWelcome to {BoringVersionInformation.FullVersion}\n");
        
        this._terminal.WriteString($"  Boot took {this._sysTimer.ElapsedNanoseconds}ns\n");

        List<Program> programs = this.InstantiatePrograms();

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
    
    public bool KernelIsRunning = true;

    public bool HaltKernel()
    {
        this.KernelIsRunning = false;
        return true;
    }
}