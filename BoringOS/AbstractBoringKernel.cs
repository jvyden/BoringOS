using BoringOS.Network;
using BoringOS.Programs;
using BoringOS.Terminal;
using BoringOS.Threading;
using BoringOS.Time;

namespace BoringOS;

public abstract partial class AbstractBoringKernel
{
    protected ITerminal KernelTerminal = null!;

    private KernelTimer _sysTimer = null!;

    protected abstract bool NeedsManualGarbageCollection { get; }
    
    public abstract long CollectGarbage();
    public abstract long GetUsedMemory();
    protected abstract void WriteAll(string message);
    protected abstract SystemInformation CollectSystemInfo();

    protected virtual ITerminal InstantiateKernelTerminal() => new ConsoleTerminal();
    public virtual KernelTimer InstantiateTimer() => new UtcNowKernelTimer();
    protected abstract NetworkManager InstantiateNetworkManager();
    protected abstract AbstractProcessManager InstantiateProcessManager();

    private partial List<Program> InstantiatePrograms();

    protected virtual void StartUserspace(List<Program> programs)
    {
        ConsoleTerminal terminal = new ConsoleTerminal();
        StartSession(terminal, programs);

        while (this.KernelIsRunning)
        {}
    }

    protected void StartSession(ITerminal terminal, List<Program> programs)
    {
        this.ProcessManager.StartProcess(() =>
        {
            this.KernelTerminal.WriteChar('\n');
            this.KernelTerminal.WriteString($"Welcome to {BoringVersionInformation.FullVersion}\n");
            this.KernelTerminal.WriteString($"  Boot took {this._sysTimer.ElapsedMilliseconds}ms\n");
            
            BoringSession session = new BoringSession(terminal, programs, this);
            BoringShell shell = new BoringShell(session);
            while (this.KernelIsRunning)
            {
                shell.InputCycle();
            }
        });
    }

    public long ElapsedMilliseconds => this._sysTimer.ElapsedMilliseconds;
    public SystemInformation SystemInformation { get; private set; }
    public NetworkManager Network { get; private set; } = null!;
    public AbstractProcessManager ProcessManager { get; private set; } = null!;

    public void OnBoot()
    {
        this._sysTimer = this.InstantiateTimer();
        this._sysTimer.Start();
    }

    public void BeforeRun()
    {
        // Set up terminal
        Console.WriteLine("  Initializing terminal");
        this.KernelTerminal = this.InstantiateKernelTerminal();
        
        this.KernelTerminal.WriteString("  Gathering SystemInformation\n");
        this.SystemInformation = this.CollectSystemInfo();

        this.KernelTerminal.WriteString($"    CPU: {this.SystemInformation.CPUVendor} {this.SystemInformation.CPUBrand}, ");
        this.KernelTerminal.WriteString($"{this.SystemInformation.MemoryCountKilobytes / 1024}MB of memory\n");

        this.KernelTerminal.WriteString("  Initializing network\n");
        this.Network = this.InstantiateNetworkManager();
        this.Network.Initialize();
        
        this.KernelTerminal.WriteString("  Initializing threading\n");
        this.ProcessManager = this.InstantiateProcessManager();
        this.ProcessManager.Initialize();

        if (this.NeedsManualGarbageCollection)
        {
            long freed = this.CollectGarbage();
            this.KernelTerminal.WriteString($"  Freed {freed / 1024}KB of memory\n");
        }
    }

    public void Run()
    {
        try
        {
            List<Program> programs = this.InstantiatePrograms();
            this.StartUserspace(programs);
        }
        catch(Exception e)
        {
            try
            {
                this.HandleCrash(e);
            }
            catch(Exception ee)
            {
                this.WriteAll("Could not properly handle crash. Halt.");
                this.WriteAll(ee.ToString());
                this.HaltKernel();
                return;
            }
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

    protected virtual void PrintException(Exception e)
    {
        while (true)
        {
            this.KernelTerminal.WriteString(e.ToString());
            this.KernelTerminal.WriteChar('\n');
            if (e.InnerException != null)
            {
                e = e.InnerException;
                continue;
            }

            break;
        }
    }

    private void HandleCrash(Exception e)
    {
        // TODO: use terminal if possible
        this.WriteAll("Unhandled exception: " + e + '\n');
        this.WriteAll("Crash occurred - please see kernel terminal for instructions\n");
        // this.KernelTerminal.BackgroundColor = ConsoleColor.DarkRed;
        // this.KernelTerminal.ForegroundColor = ConsoleColor.White;
        this.KernelTerminal.ClearScreen();
        this.KernelTerminal.SetCursorPosition(0, 0);
        
        this.KernelTerminal.WriteString(BoringVersionInformation.FullVersion);
        
        this.KernelTerminal.WriteChar('\n');
        PrintException(e);
        this.KernelTerminal.WriteChar('\n');
        
        this.KernelTerminal.WriteString("The above exception went entirely unhandled - the kernel had to step in.\n");
        this.KernelTerminal.WriteString("This is a particularly bad crash, as it should have been handled appropriately\nby the session.");
        this.KernelTerminal.WriteString(" Alas, it was not and you have now been brought here.\n");
        
        this.KernelTerminal.WriteChar('\n');
        this.KernelTerminal.WriteString("If you do not know what you are doing, press 'H' to halt the system now.");
        this.KernelTerminal.WriteChar('\n');
        
        while (true)
        {
            this.KernelTerminal.WriteString("Press H to halt, D to take a memory dump, or C to continue: ");
            char c = this.KernelTerminal.ReadKey().KeyChar;

            if (c == 'c')
            {
                // Console.BackgroundColor = ConsoleColor.Black;
                // Console.ForegroundColor = ConsoleColor.Gray;
                this.KernelTerminal.ClearScreen();
                this.KernelTerminal.WriteString("You have been dumped back to wherever you were.\n");
                this.KernelTerminal.WriteString("If any problems persist, please halt and reboot the computer.\n");
                this.KernelTerminal.WriteChar('\n');
                break;
            }

            if (c == 'h')
            {
                this.HaltKernel();
                break;
            }
            
            if (c == 'd')
            {
                this.KernelTerminal.WriteString("Unimplemented\n");
            }
            
#if DEBUG
            if (c == '0')
            {
                this.KernelTerminal.WriteString("Crashing again on purpose\n");
                throw e;
            }
#endif

            this.KernelTerminal.WriteString("Invalid key\n");
        }
    }
}