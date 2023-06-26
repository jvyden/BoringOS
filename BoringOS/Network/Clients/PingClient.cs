using BoringOS.Network.Data;

namespace BoringOS.Network.Clients;

public abstract class PingClient : INetworkClient
{
    public abstract PingReply PingOnce(IpAddress target, ushort bytes = 64);
    
    public abstract void Dispose();
}