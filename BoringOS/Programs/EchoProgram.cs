using System;

namespace BoringOS.Programs;

public class EchoProgram : Program
{
    public EchoProgram() : base("echo")
    {}

    public override byte Invoke(string[] args, BoringSession session)
    {
        session.Terminal.WriteString(string.Join(' ', args));
        return 0;
    }
}