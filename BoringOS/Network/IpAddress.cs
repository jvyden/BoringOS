namespace BoringOS.Network;

public readonly struct IpAddress
{
    public static readonly IpAddress Default = new(0, 0, 0, 0);
    
    private readonly byte _a;
    private readonly byte _b;
    private readonly byte _c;
    private readonly byte _d;

    public IpAddress(byte a, byte b, byte c, byte d)
    {
        this._a = a;
        this._b = b;
        this._c = c;
        this._d = d;
    }

    public override string ToString()
    {
        const char separator = '.'; 
        const int maxLength = 15; // "255.255.255.255".Length
        Span<char> address = stackalloc char[maxLength];

        int offset = 0;
        this._a.TryFormat(address, out int written);
        offset += written;
        address[offset++] = separator;
        
        this._b.TryFormat(address[offset..], out written);
        offset += written;
        address[offset++] = separator;
        
        this._c.TryFormat(address[offset..], out written);
        offset += written;
        address[offset++] = separator;
        
        this._d.TryFormat(address[offset..], out _);
        
        return address.ToString();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this._a, this._b, this._c, this._d);
    }

    public bool Equals(IpAddress other) => this._a == other._a && this._b == other._b && this._c == other._c && this._d == other._d;

    public bool Equals(string other) => this.ToString() == other;

    public override bool Equals(object? obj)
    {
        return obj is IpAddress other && Equals(other);
    }
    
    public static bool operator ==(IpAddress left, IpAddress right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(IpAddress left, IpAddress right)
    {
        return !left.Equals(right);
    }
}