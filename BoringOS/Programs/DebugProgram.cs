using BoringOS.Terminal;

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

    public override byte Invoke(string[] args, BoringSession session)
    {
        if (args.Length == 0) return ShowHelp(session.Terminal);

        if (args[0] == "crash")
        {
            throw new Exception("Manually triggered exception");
        }

        if(args[0] == "ret")
        {
            if (args.Length < 2) return ShowHelp(session.Terminal);
            return (byte)ushort.Parse(args[1].Trim());
        }

        if (args[0] == "keylog")
        {
            KeyLog(session.Terminal);
        }

        return 0;
    }
}