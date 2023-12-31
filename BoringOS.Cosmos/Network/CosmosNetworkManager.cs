using System;
using BoringOS.Cosmos.Network.Clients;
using BoringOS.Network;
using BoringOS.Network.Clients;
using Cosmos.HAL;
using Cosmos.HAL.Network;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DHCP;

namespace BoringOS.Cosmos.Network;

public class CosmosNetworkManager : NetworkManager
{
    protected override void InitializeInternal()
    {
        NetworkInit.Init();

        Console.WriteLine("    Network stack initialized");

        if (NetworkDevice.Devices.Count <= 0)
        {
            Console.WriteLine("    No network devices were found. Configuration cannot continue.");
            return;
        }

        Console.Write("    DHCP Initialization time: ");
        
        int time;
        using (DHCPClient client = new())
        {
            time = client.SendDiscoverPacket();
        }
        
        Console.Write(time);
        Console.WriteLine("ms");
        
        NetworkDevice network = NetworkDevice.GetDeviceByName("eth0");
        Address address;
        
        if (NetworkConfiguration.CurrentNetworkConfig != null && time != -1)
        {
            Console.Write("    Current address: ");
            Console.WriteLine(NetworkConfiguration.CurrentAddress);
            address = NetworkConfiguration.CurrentAddress;
        }
        else
        {
            Console.WriteLine("    An IP address was not obtained. Setting one manually...");

            Address ip = new Address(192, 168, 1, 157);
            Address mask = new Address(255, 255, 255, 0);
            Address gateway = new Address(192, 168, 1, 1);
            
            IPConfig.Enable(network, ip, mask, gateway);
            
            Console.Write("    Manually set IP to ");
            Console.WriteLine(ip);
            address = ip;
        }
        
        this.AddAdapter(new CosmosNetworkAdapter(network, address));
    }

    public override PingClient GetPingClient() => new CosmosPingClient();
}