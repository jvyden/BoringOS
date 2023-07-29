namespace BoringOS.Network;

public abstract class NetworkAdapter
{
    public abstract string DeviceName { get; }
    public abstract string Identifier { get; }
    public IpAddress IpAddress { get; protected set; } = IpAddress.Default;
    public abstract MacAddress MacAddress { get; protected set; } 
}