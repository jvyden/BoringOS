using BoringOS.Emulator.Network.Clients;
using BoringOS.Network;
using BoringOS.Network.Clients;

namespace BoringOS.Emulator.Network;

public class EmulatedNetworkManager : NetworkManager
{
    protected override void InitializeInternal()
    {
        this.AddAdapter(new EmulatedNetworkAdapter("lo", MacAddress.Default, IpAddress.Local));
    }

    public override PingClient GetPingClient() => new EmulatedPingClient();
}