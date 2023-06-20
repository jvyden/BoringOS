namespace BoringOS.Programs;

public class ClearScreenProgram : Program
{
    public ClearScreenProgram() : base("clear", "Clears the screen")
    {
    }

    public override byte Invoke(string[] args, BoringSession session)
    {
        session.Terminal.ClearScreen();
        return 0;
    }
}