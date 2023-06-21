using System;
using BoringOS.Network;
using Cosmos.HAL;
using Cosmos.HAL.Network;
using Cosmos.System.Network;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DHCP;

namespace BoringOS.Kernel.Network;

public class CosmosNetworkManager : NetworkManager
{
    protected override void InitializeInternal()
    {
        NetworkInit.Init();
        // NetworkStack.Initialize(); (already done in Global.Init)
        
        Console.WriteLine("    Network stack initialized");

        if (NetworkDevice.Devices.Count <= 0)
        {
            Console.WriteLine("    No network devices were found. Configuration cannot continue.");
            return;
        }

        int time;
        using (DHCPClient client = new())
        {
            time = client.SendDiscoverPacket();
        }

        Console.Write("    DHCP Initialization time: ");
        Console.Write(time);
        Console.WriteLine("ms");
        
        NetworkDevice network = NetworkDevice.GetDeviceByName("eth0");
        
        if (NetworkConfiguration.CurrentNetworkConfig != null && time != -1)
        {
            Console.Write("    Current address: ");
            Console.WriteLine(NetworkConfiguration.CurrentAddress);
        }
        else
        {
            Console.WriteLine("    An IP address was not obtained. Setting one manually...");

            Address ip = new Address(192, 168, 1, 157);
            Address subnet = new Address(255, 255, 255, 0);
            Address gateway = new Address(192, 168, 1, 1);
            
            IPConfig.Enable(network, ip, subnet, gateway);
            
            Console.Write("    Manually set IP to ");
            Console.WriteLine(ip);
        }

        Console.WriteLine(network.Ready);
        // network.Enable();
    }
}