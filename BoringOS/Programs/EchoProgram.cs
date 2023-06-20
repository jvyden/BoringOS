using System;

namespace BoringOS.Programs;

public class EchoProgram : Program
{
    public EchoProgram() : base("echo")
    {}

    public override byte Invoke(string[] args, BoringSession session)
    {
        foreach (string arg in args)
        {
            session.Terminal.WriteString(arg);
            session.Terminal.WriteChar(' ');
        }
        session.Terminal.WriteChar('\n');

        return 0;
    }
}