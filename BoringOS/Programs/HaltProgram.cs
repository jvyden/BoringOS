

namespace BoringOS.Programs;

public class HaltProgram : Program
{
    public HaltProgram() : base("halt", "Immediately halts the system.") {}

    public override byte Invoke(string[] args, BoringSession session)
    {
        session.Terminal.WriteString("Requesting a halt... ");
        bool success = session.Kernel.HaltKernel();
        session.Terminal.WriteString(success ? "OK" : "FAIL");
        session.Terminal.WriteChar('\n');
        return 0;
    }
}