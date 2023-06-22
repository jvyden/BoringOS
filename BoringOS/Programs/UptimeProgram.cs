namespace BoringOS.Programs;

public class UptimeProgram : Program
{
    public UptimeProgram() : base("uptime")
    {}

    public override byte Invoke(string[] args, BoringSession session)
    {
        TimeSpan time = TimeSpan.FromMilliseconds(session.Kernel.ElapsedMilliseconds);
        
        session.Terminal.WriteString($"Up for {time.Days} days, {time.Hours} hours, {time.Minutes} minutes, {time.Seconds} seconds\n");
        return 0;
    }
}