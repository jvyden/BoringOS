using BoringOS.Network;

namespace BoringOS.Emulator.Network;

public class EmulatedNetworkAdapter : NetworkAdapter
{
    public EmulatedNetworkAdapter(string identifier, MacAddress macAddress, IpAddress address)
    {
        this.Identifier = identifier;
        this.MacAddress = macAddress;
        this.IpAddress = address;
    }

    public override string DeviceName => "Emulated Network Adapter";
    public override string Identifier { get; }
    public sealed override MacAddress MacAddress { get; protected init; }
}