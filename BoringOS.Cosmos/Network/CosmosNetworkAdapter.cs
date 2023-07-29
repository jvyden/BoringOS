using BoringOS.Network;
using Cosmos.HAL;
using Cosmos.System.Network.IPv4;

namespace BoringOS.Cosmos.Network;

public class CosmosNetworkAdapter : NetworkAdapter
{
    public CosmosNetworkAdapter(NetworkDevice device, Address address)
    {
        this.DeviceName = device.Name;
        this.Identifier = device.NameID;

        byte[] b = device.MACAddress.bytes;
        this.MacAddress = new MacAddress(b[0], b[1], b[2], b[3], b[4], b[5]);
        
        // TODO: allow updates to this when it changes
        b = address.ToByteArray();
        this.IpAddress = new IpAddress(b[0], b[1], b[2], b[3]);
    }
    
    public override string Identifier { get; }
    public override string DeviceName { get; }
    public sealed override MacAddress MacAddress { get; protected set; }
}