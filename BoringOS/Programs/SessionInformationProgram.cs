namespace BoringOS.Programs;

public class SessionInformationProgram : Program
{
    public SessionInformationProgram() : base("session", "Describes your current session")
    {}

    public override byte Invoke(string[] args, BoringSession session)
    {
        session.Terminal.WriteString($"You are session {session.SessionId}\n");
        session.Terminal.WriteString($"Session has been alive for {session.ElapsedMilliseconds}ms\n");
        session.Terminal.WriteString($"Kernel has been up for {session.Kernel.ElapsedMilliseconds}ms\n");
        return 0;
    }
}