using Cosmos.Core;

namespace BoringOS.Programs;

public class HaltProgram : Program
{
    public HaltProgram() : base("halt", "Immediately halts the system.") {}

    public override byte Invoke(string[] args, BoringSession session)
    {
        if (args.Length >= 1 && args[0] == "--now")
        {
            session.Terminal.WriteString("HALTING THE SYSTEM NOW!");
            CPU.Halt();
            return 1;
        }

        session.Terminal.WriteString("Requesting a halt... ");
        bool success = session.RequestKernelHalt();
        session.Terminal.WriteString(success ? "OK" : "FAIL");
        session.Terminal.WriteChar('\n');
        return 0;
    }
}