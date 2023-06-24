namespace BoringOS.Network;

public readonly struct IpAddress
{
    public static readonly IpAddress Default = new(0, 0, 0, 0);
    public static readonly IpAddress Local = new(127, 0, 0, 1);
    
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

    public IpAddress(ReadOnlySpan<char> ip)
    {
        int offset = 0;

        offset += ParseSection(ip[offset..], out _a);
        offset += ParseSection(ip[offset..], out _b);
        offset += ParseSection(ip[offset..], out _c);
        ParseSection(ip[offset..], out _d, true);
    }

    private static int ParseSection(ReadOnlySpan<char> ip, out byte b, bool last = false)
    {
        const char separator = '.'; 
        const int maxSectionLength = 3; // "255".Length
        
        Span<char> section = stackalloc char[maxSectionLength];
        int i = 0;

        for (int charIndex = 0; charIndex < ip.Length; charIndex++)
        {
            char c = ip[charIndex];
            if (c != separator)
            {
                if (i >= maxSectionLength) throw new FormatException("Invalid IP address");
                section[i] = c;
            }
            else
            {
                b = byte.Parse(section);
                // b = 1;
                return i + 1;
            }

            i++;
        }

        if (!last) throw new FormatException("Section contains no characters");
        
        b = byte.Parse(section);
        // b = 1;
        return 0;
    }

    public override string ToString()
    {
        // TODO: Implement MOSA
#if !DEBUGMOSA
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
#else
        return this._a + '.' + this._b + '.' + this._c + '.' + this._d.ToString();
#endif
    }

    public override int GetHashCode()
    {
        int hash = 0;
        hash |= (this._a & 0xFF) << 24;
        hash |= (this._b & 0xFF) << 16;
        hash |= (this._c & 0xFF) << 8;
        hash |= (this._d & 0xFF);
        return hash;
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