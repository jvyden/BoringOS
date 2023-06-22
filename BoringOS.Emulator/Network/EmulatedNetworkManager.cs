using BoringOS.Network;

namespace BoringOS.Emulator.Network;

public class EmulatedNetworkManager : NetworkManager
{
    protected override void InitializeInternal()
    {
        this.AddAdapter(new EmulatedNetworkAdapter("lo", MacAddress.Default, new IpAddress(127, 0, 0, 1)));
    }
}