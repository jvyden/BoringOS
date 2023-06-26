using System.Net.NetworkInformation;
using System.Text;
using BoringOS.Network;
using BoringOS.Network.Clients;
using PingReply = BoringOS.Network.Data.PingReply;

namespace BoringOS.Emulator.Network.Clients;

public class EmulatedPingClient : PingClient
{
    private readonly Ping _ping = new();
    
    public override PingReply PingOnce(IpAddress target, ushort bytes = 64)
    {
        // can't do raw payload, ignore bytes
        // byte[] data = Encoding.ASCII.GetBytes(new string('a', bytes));
        
        var ping = this._ping.Send(target.ToString(), 1000);
        NetworkResult status = ping.Status == IPStatus.Success ? NetworkResult.Ok : NetworkResult.Failure;
        return new PingReply(status, target, ping.RoundtripTime, ping.Buffer);
    }

    public override void Dispose()
    {
        this._ping.Dispose();
        GC.SuppressFinalize(this);
    }
}