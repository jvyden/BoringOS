using BoringOS.Network;
using BoringOS.Programs;
using BoringOS.Terminal;
using BoringOS.Threading;
using BoringOS.Time;

namespace BoringOS;

#nullable disable

public abstract partial class BoringKernel
{
    protected ITerminal KernelTerminal = null!;

    private KernelTimer _sysTimer = null!;

    protected abstract bool NeedsManualGarbageCollection { get; }
    
    public abstract long CollectGarbage();
    public abstract long GetUsedMemory();
    protected abstract void WriteAll(string message);
    protected abstract SystemInformation CollectSystemInfo();

    #if !DEBUGMOSA
    protected virtual ITerminal InstantiateKernelTerminal() => new ConsoleTerminal();
    #else
    protected abstract ITerminal InstantiateKernelTerminal();
    #endif
    public virtual KernelTimer InstantiateTimer() => new UtcNowKernelTimer();
    protected abstract NetworkManager InstantiateNetworkManager();
    protected abstract ProcessManager InstantiateProcessManager();

    private partial List<Program> InstantiatePrograms();

    protected virtual void StartUserspace(List<Program> programs)
    {
        ITerminal terminal = this.InstantiateKernelTerminal();
        StartSession(terminal, programs);

        while (this.KernelIsRunning)
        {}
    }

    protected void StartSession(ITerminal terminal, List<Program> programs)
    {
        this.ProcessManager.StartProcess(() =>
        {
            terminal.WriteChar('\n');
            terminal.WriteString($"Welcome to {BoringVersionInformation.FullVersion}\n");
            terminal.WriteString($"  Boot took {this._sysTimer.ElapsedMilliseconds}ms\n");
            
            BoringSession session = new BoringSession(terminal, programs, this);
            BoringShell shell = new BoringShell(session);
            while (this.KernelIsRunning)
            {
                try
                {
                    shell.InputCycle();
                }
                catch(Exception e)
                {
                    HandleCrash(e, terminal);
                }
            }
        });
    }

    public long ElapsedMilliseconds => this._sysTimer.ElapsedMilliseconds;
    public SystemInformation SystemInformation { get; private set; }
    public NetworkManager Network { get; private set; } = null!;
    public ProcessManager ProcessManager { get; private set; } = null!;

    public void OnBoot()
    {
        this._sysTimer = this.InstantiateTimer();
        this._sysTimer.Start();
    }

    public void BeforeRun()
    {
        // Set up terminal
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
                this.HandleCrash(e, this.KernelTerminal);
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

    private void HandleCrash(Exception e, ITerminal terminal)
    {
        this.WriteAll("Unhandled exception: " + e + '\n');
        this.WriteAll("Crash occurred - please see kernel terminal for instructions\n");
        // terminal.BackgroundColor = ConsoleColor.DarkRed;
        // terminal.ForegroundColor = ConsoleColor.White;
        terminal.ClearScreen();
        terminal.SetCursorPosition(0, 0);
        
        terminal.WriteString(BoringVersionInformation.FullVersion);
        
        terminal.WriteChar('\n');
        PrintException(e);
        terminal.WriteChar('\n');
        
        terminal.WriteString("The above exception went entirely unhandled - the kernel had to step in.\n");
        terminal.WriteString("This is a particularly bad crash, as it should have been handled appropriately\nby the session.");
        terminal.WriteString(" Alas, it was not and you have now been brought here.\n");
        
        terminal.WriteChar('\n');
        terminal.WriteString("If you do not know what you are doing, press 'H' to halt the system now.");
        terminal.WriteChar('\n');
        
        while (true)
        {
            terminal.WriteString("Press H to halt, D to take a memory dump, or C to continue: ");
            char c = terminal.ReadKey().KeyChar;

            if (c == 'c')
            {
                // Console.BackgroundColor = ConsoleColor.Black;
                // Console.ForegroundColor = ConsoleColor.Gray;
                terminal.ClearScreen();
                terminal.WriteString("You have been dumped back to wherever you were.\n");
                terminal.WriteString("If any problems persist, please halt and reboot the computer.\n");
                terminal.WriteChar('\n');
                break;
            }

            if (c == 'h')
            {
                this.HaltKernel();
                break;
            }
            
            if (c == 'd')
            {
                terminal.WriteString("Unimplemented\n");
            }
            
#if DEBUG
            if (c == '0')
            {
                terminal.WriteString("Crashing again on purpose\n");
                throw e;
            }
#endif

            terminal.WriteString("Invalid key\n");
        }
    }
}