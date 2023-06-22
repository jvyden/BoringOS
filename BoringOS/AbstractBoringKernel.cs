using BoringOS.Network;
using BoringOS.Programs;
using BoringOS.Terminal;
using BoringOS.Time;

namespace BoringOS;

public abstract partial class AbstractBoringKernel
{
    private ITerminal _terminal = null!;
    private BoringShell _shell = null!;
    private BoringSession _session = null!;

    private KernelTimer _sysTimer = null!;

    protected abstract bool NeedsManualGarbageCollection { get; }
    
    public abstract long CollectGarbage();
    public abstract long GetAllocatedMemory();
    protected abstract void WriteAll(string message);
    protected abstract SystemInformation CollectSystemInfo();

    protected virtual ITerminal InstantiateTerminal() => new ConsoleTerminal();
    public virtual KernelTimer InstantiateTimer() => new UtcNowKernelTimer();
    protected abstract NetworkManager InstantiateNetworkManager();
    
    private partial List<Program> InstantiatePrograms();

    public long ElapsedMilliseconds => this._sysTimer.ElapsedMilliseconds;
    public SystemInformation SystemInformation { get; private set; }
    public NetworkManager Network { get; private set; } = null!;

    public void OnBoot()
    {
        this._sysTimer = this.InstantiateTimer();
        this._sysTimer.Start();
    }

    public void BeforeRun()
    {
        Console.WriteLine("  Gathering SystemInformation");
        this.SystemInformation = this.CollectSystemInfo();

        Console.Write($"    CPU: {this.SystemInformation.CPUVendor} {this.SystemInformation.CPUBrand}, ");
        Console.WriteLine($"{this.SystemInformation.MemoryCountMegabytes}MB of upper memory");

        if (this.NeedsManualGarbageCollection)
        {
            long freed = this.CollectGarbage();
            Console.WriteLine($"  Freed {freed / 1048576}MB of memory");
        }
        
        Console.WriteLine("  Initializing network");
        this.Network = this.InstantiateNetworkManager();
        this.Network.Initialize();
        
        // Set up terminal
        Console.WriteLine("  Initializing terminal");
        this._terminal = this.InstantiateTerminal();
        
        this._terminal.WriteChar('\n');
        this._terminal.WriteString($"Welcome to {BoringVersionInformation.FullVersion}\n");
        this._terminal.WriteString($"  Boot took {this._sysTimer.ElapsedMilliseconds}ms\n");

        List<Program> programs = this.InstantiatePrograms();

        this._session = new BoringSession(this._terminal, programs, this);
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

    private void HandleCrash(Exception e)
    {
        // TODO: use terminal if possible
        this.WriteAll("Unhandled exception: " + e);
        this.WriteAll("Crash occurred - please see console for instructions\n");
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        
        Console.WriteLine(BoringVersionInformation.FullVersion);
        
        Console.WriteLine();
        Console.WriteLine(e);
        Console.WriteLine();
        
        Console.WriteLine("The above exception went entirely unhandled - the kernel had to step in.");
        Console.Write("This is a particularly bad crash, as it should have been handled appropriately\nby the session.");
        Console.Write(" Alas, it was not and you have now been brought here.\n");
        
        Console.WriteLine();
        Console.WriteLine("If you do not know what you are doing, press 'H' to halt the system now.");
        Console.WriteLine();
        
        while (true)
        {
            Console.Write("Press H to halt, D to take a memory dump, or C to continue: ");
            char c = Console.ReadKey(true).KeyChar;

            if (c == 'c')
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Clear();
                Console.WriteLine("You have been dumped back to wherever you were.");
                Console.WriteLine("If any problems persist, please halt and reboot the computer.");
                Console.WriteLine();
                break;
            }

            if (c == 'h')
            {
                this.HaltKernel();
                break;
            }
            
            if (c == 'd')
            {
                Console.WriteLine("Unimplemented");
            }
            
            #if DEBUG
            if (c == '0')
            {
                throw new Exception("Double crash!");
            }
            #endif

            Console.WriteLine("Invalid key");
        }
    }
}