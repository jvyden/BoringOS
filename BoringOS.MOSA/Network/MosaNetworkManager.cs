using BoringOS.Network;
using BoringOS.Network.Clients;

namespace BoringOS.MOSA.Network;

public class MosaNetworkManager : NetworkManager
{
    protected override void InitializeInternal()
    {
        // Do nothing for now
    }

    public override PingClient GetPingClient()
    {
        return null;
    }
}