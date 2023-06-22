namespace BoringOS.Programs;

public class SysInfoProgram : Program
{
    public SysInfoProgram() : base("sysinfo", "Fetches system information")
    {
    }

    public override byte Invoke(string[] args, BoringSession session)
    {
        long allocatedMemory = session.Kernel.GetAllocatedMemory() / 1024;
        
        session.Terminal.WriteString(BoringVersionInformation.FullVersion);
        session.Terminal.WriteChar('\n');
        session.Terminal.WriteString($"CPU: {session.Kernel.SystemInformation.CPUVendor} {session.Kernel.SystemInformation.CPUBrand}\n");
        session.Terminal.WriteString($"MEM: {allocatedMemory}KB/{session.Kernel.SystemInformation.MemoryCountKilobytes}KB\n");
        return 0;
    }
}