using BoringOS.Network;
using Cosmos.System.Network.IPv4;

namespace BoringOS.Cosmos.Network;

public static class IpAddressExtensions
{
    public static Address ToCosmosAddress(this IpAddress address)
    {
        return new Address(address.A, address.B, address.C, address.D);
    }
}