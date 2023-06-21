namespace BoringOS.Programs;

public class SysInfoProgram : Program
{
    public SysInfoProgram() : base("sysinfo", "Fetches system information")
    {
    }

    public override byte Invoke(string[] args, BoringSession session)
    {
        long allocatedMemory = session.Kernel.GetAllocatedMemory() / 1048576;
        
        session.Terminal.WriteString(BoringVersionInformation.FullVersion);
        session.Terminal.WriteChar('\n');
        session.Terminal.WriteString($"CPU: {session.SystemInformation.CPUVendor} {session.SystemInformation.CPUBrand}\n");
        session.Terminal.WriteString($"MEM: {allocatedMemory}MB/{session.SystemInformation.MemoryCountMegabytes}MB\n");
        return 0;
    }
}