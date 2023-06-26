using BoringOS.Network;
using BoringOS.Network.Clients;
using BoringOS.Network.Data;
using BoringOS.Terminal;

namespace BoringOS.Programs;

public class NetworkManagementProgram : Program
{
    public NetworkManagementProgram() : base("net", "Configure the network")
    {}
    
    private static byte ShowHelp(ITerminal terminal)
    {
        terminal.WriteString("Unknown subcommand or bad invocation\n");
        terminal.WriteChar('\n');
        terminal.WriteString("ls: List adapters\n");
        terminal.WriteString("ping: Time how long it takes for a round trip\n");
#if DEBUG
        terminal.WriteString("dbgip: Parse and serialize IP\n");
#endif

        return 1;
    }

    private static void ShowAdapters(ITerminal terminal, IEnumerable<NetworkAdapter> adapters)
    {
        terminal.WriteString("Attached network adapters:\n\n");
        foreach (NetworkAdapter adapter in adapters)
        {
            terminal.WriteString($"{adapter.Identifier}: {adapter.MacAddress} ({adapter.DeviceName})\n");
            terminal.WriteString($"  {adapter.IpAddress}\n");
        }
    }

    private static void PingIp(ITerminal terminal, string ip, NetworkManager network)
    {
        IpAddress target = new IpAddress(ip);
        
        using PingClient client = network.GetPingClient();
        PingReply reply = client.PingOnce(target);
        if (reply.Result == NetworkResult.Ok)
        {
            terminal.WriteString($"{reply.ByteCount} bytes from {reply.Source}: {reply.RoundtripTime}ms\n");
        }
        else
        {
            terminal.WriteString(reply.Result.ToString());
        }
    }

    private static void DebugIp(ITerminal terminal, string ip)
    {
        IpAddress address = new IpAddress(ip);
        Thread.Sleep(1000);
        terminal.WriteString(address.ToString());
        terminal.WriteChar('\n');
    }

    public override byte Invoke(string[] args, BoringSession session)
    {
        if (args.Length == 0) return ShowHelp(session.Terminal);
        if (args[0] == "ls") ShowAdapters(session.Terminal, session.Kernel.Network.GetAdapters());
        if(args[0] == "ping") PingIp(session.Terminal, args[1], session.Kernel.Network);
        if (args[0] == "dbgip" && args.Length >= 2) DebugIp(session.Terminal, args[1]);

        return 0;
    }
}