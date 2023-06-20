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

        return 1;
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

        return 0;
    }
}