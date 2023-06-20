namespace BoringOS.Programs;

public class HelpProgram : Program
{
    public HelpProgram() : base("help") {}

    public override byte Invoke(string[] args, BoringSession session)
    {
        foreach (Program program in session.Programs)
        {
            session.Terminal.WriteString($"{program.Name}: {program.Description}\n");
        }

        return 0;
    }
}