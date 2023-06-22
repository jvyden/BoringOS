namespace BoringOS.Network;

public readonly struct MacAddress
{
    public static readonly MacAddress Default = new(0, 0, 0, 0, 0, 0);
    
    private readonly byte _a;
    private readonly byte _b;
    private readonly byte _c;
    private readonly byte _d;
    private readonly byte _e;
    private readonly byte _f;
    
    public MacAddress(byte a, byte b, byte c, byte d, byte e, byte f)
    {
        this._a = a;
        this._b = b;
        this._c = c;
        this._d = d;
        this._e = e;
        this._f = f;
    }
    
    public override string ToString()
    {
        const char separator = ':';
        const int maxLength = 17; // "01:23:45:67:89:AB".Length
        Span<char> address = stackalloc char[maxLength];

        ReadOnlySpan<char> format = "X2".AsSpan();

        int offset = 0;
        this._a.TryFormat(address, out int written, format);
        offset += written;
        address[offset++] = separator;
        
        this._b.TryFormat(address[offset..], out written, format);
        offset += written;
        address[offset++] = separator;
        
        this._c.TryFormat(address[offset..], out written, format);
        offset += written;
        address[offset++] = separator;
        
        this._d.TryFormat(address[offset..], out written, format);
        offset += written;
        address[offset++] = separator;
        
        this._e.TryFormat(address[offset..], out written, format);
        offset += written;
        address[offset++] = separator;
        
        this._f.TryFormat(address[offset..], out _, format);
        
        return address.ToString();
    }
}