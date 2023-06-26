namespace BoringOS.Network.Data;

public readonly struct PingReply
{
    public readonly NetworkResult Result;
    public readonly IpAddress Source;
    public readonly long RoundtripTime;
    public readonly ushort ByteCount;

    public PingReply(NetworkResult result, IpAddress ipAddress, long roundtripTime, IReadOnlyCollection<byte> buffer)
    {
        this.Result = result;
        this.Source = ipAddress;
        this.RoundtripTime = roundtripTime;
        this.ByteCount = (ushort)buffer.Count;
    }
}