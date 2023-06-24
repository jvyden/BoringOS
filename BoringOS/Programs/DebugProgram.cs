using BoringOS.Terminal;
using BoringOS.Threading;

namespace BoringOS.Programs;

public class DebugProgram : Program
{
    public DebugProgram() : base("dbg", "Debugging commands")
    {}

    private static byte ShowHelp(ITerminal terminal)
    {
        terminal.WriteString("Unknown subcommand or bad invocation\n");
        terminal.WriteChar('\n');
        terminal.WriteString("crash: Throw exception\n");
        terminal.WriteString("ret: Return a status code\n");
        terminal.WriteString("keylog: Log keyboard input from terminal\n");

        return 1;
    }

    private static void KeyLog(ITerminal terminal)
    {
        terminal.WriteString("Press enter to exit.\n");
        
        while (true)
        {
            ConsoleKeyInfo key = terminal.ReadKey();
            terminal.WriteString("0x");
            terminal.WriteString(((byte)key.KeyChar).ToString("x2"));
            terminal.WriteChar('\n');

            if (key.KeyChar is '\n' or '\r') break;
            if (key.Key is ConsoleKey.Enter) break;
        }
    }

    private static void SpawnThread(ITerminal terminal, ProcessManager processManager)
    {
        processManager.StartProcess(() =>
        {
            int i = 0;
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Clear();
                terminal.WriteString($"Thread tick {i++}\n");
                processManager.Sleep(1000);
            }
        });
    }

    public override byte Invoke(string[] args, BoringSession session)
    {
        #if RELEASE
        return 1;
        #endif
        
        if (args.Length == 0) return ShowHelp(session.Terminal);

        switch (args[0])
        {
            case "ret" when args.Length < 2:
            default:
                return ShowHelp(session.Terminal);
            case "crash":
                throw new Exception("Manually triggered exception");
            case "ret":
                return (byte)ushort.Parse(args[1].Trim());
            case "keylog":
                KeyLog(session.Terminal);
                break;
            case "thread":
                SpawnThread(session.Terminal, session.Kernel.ProcessManager);
                break;
        }

        return 0;
    }
}