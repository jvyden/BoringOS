namespace BoringOS.Network;

public readonly struct IpAddress
{
    public static readonly IpAddress Default = new(0, 0, 0, 0);
    public static readonly IpAddress Local = new(127, 0, 0, 1);
    
    public readonly byte A;
    public readonly byte B;
    public readonly byte C;
    public readonly byte D;

    public IpAddress(byte a, byte b, byte c, byte d)
    {
        this.A = a;
        this.B = b;
        this.C = c;
        this.D = d;
    }

    public IpAddress(ReadOnlySpan<char> ip)
    {
        int offset = 0;

        offset += ParseSection(ip[offset..], out this.A);
        offset += ParseSection(ip[offset..], out this.B);
        offset += ParseSection(ip[offset..], out this.C);
        ParseSection(ip[offset..], out this.D, true);
    }

    private static int ParseSection(ReadOnlySpan<char> ip, out byte b, bool last = false)
    {
        #if !DEBUGMOSA
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
                // TODO: implement byte.Parse(ReadOnlySpan<char>) in COSMOS
                b = byte.Parse(section.ToString());
                // b = 1;
                return i + 1;
            }

            i++;
        }

        if (!last) throw new FormatException("Section contains no characters");
        
        b = byte.Parse(section.ToString());
        // b = 1;
        return 0;
#else
        b = 0;
        return 0;
#endif
    }

    public override string ToString()
    {
        // TODO: Implement MOSA
#if !DEBUGMOSA
        const char separator = '.';
        const int maxLength = 15; // "255.255.255.255".Length
        Span<char> address = stackalloc char[maxLength];

        int offset = 0;
        this.A.TryFormat(address, out int written);
        offset += written;
        address[offset++] = separator;
        
        this.B.TryFormat(address[offset..], out written);
        offset += written;
        address[offset++] = separator;
        
        this.C.TryFormat(address[offset..], out written);
        offset += written;
        address[offset++] = separator;
        
        this.D.TryFormat(address[offset..], out _);
        
        return address.ToString();
#else
        return this._a + '.' + this._b + '.' + this._c + '.' + this._d.ToString();
#endif
    }

    public override int GetHashCode()
    {
        int hash = 0;
        hash |= (this.A & 0xFF) << 24;
        hash |= (this.B & 0xFF) << 16;
        hash |= (this.C & 0xFF) << 8;
        hash |= (this.D & 0xFF);
        return hash;
    }

    public bool Equals(IpAddress other) => this.A == other.A && this.B == other.B && this.C == other.C && this.D == other.D;

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