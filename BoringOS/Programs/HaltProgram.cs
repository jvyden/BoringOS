using Cosmos.Core;

namespace BoringOS.Programs;

public class HaltProgram : Program
{
    public HaltProgram() : base("halt", "Immediately halts the system.") {}

    public override byte Invoke(string[] args, BoringSession session)
    {
        session.Terminal.WriteString("HALTING THE SYSTEM NOW!");
        CPU.Halt();
        return 1;
    }
}