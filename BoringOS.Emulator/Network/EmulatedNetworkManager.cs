using BoringOS.Network;

namespace BoringOS.Emulator.Network;

public class EmulatedNetworkManager : NetworkManager
{
    protected override void InitializeInternal()
    {
        this.AddAdapter(new EmulatedNetworkAdapter("lo", MacAddress.Default, IpAddress.Local));
    }
}