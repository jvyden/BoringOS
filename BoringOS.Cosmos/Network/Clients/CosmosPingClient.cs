using System;
using BoringOS.Network;
using BoringOS.Network.Clients;
using BoringOS.Network.Data;
using Cosmos.System.Network.IPv4;

namespace BoringOS.Cosmos.Network.Clients;

/// <summary>
/// This class is not thread-safe.
/// </summary>
public class CosmosPingClient : PingClient
{
    private readonly ICMPClient _client = new();
    
    public override PingReply PingOnce(IpAddress target, ushort bytes = 64)
    {
        Address cosmosTarget = target.ToCosmosAddress();
        this._client.Connect(cosmosTarget);
        this._client.SendEcho();

        // TODO: manual implementation, i don't like this API
        EndPoint endPoint = new EndPoint(cosmosTarget, 0);
        int ms = this._client.Receive(ref endPoint, 1000);
        return new PingReply(NetworkResult.Ok, target, ms, Array.Empty<byte>());
    }

    public override void Dispose()
    {
        this._client.Dispose();
        GC.SuppressFinalize(this);
    }
}