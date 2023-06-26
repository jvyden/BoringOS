using BoringOS.Network.Clients;

namespace BoringOS.Network;

public abstract class NetworkManager
{
    private bool _initialized;

    public void Initialize()
    {
        if (this._initialized) return;
        this._initialized = true;
        this.InitializeInternal();
    }

    protected abstract void InitializeInternal();

    public const int MaxAdapters = 4;
    private byte _addedAdapters = 0;
    private readonly NetworkAdapter[] _adapters = new NetworkAdapter[4];

    protected void AddAdapter(NetworkAdapter adapter)
    {
        if (this._addedAdapters == MaxAdapters - 1)
            throw new Exception($"Too many adapters added. You may only add {MaxAdapters}.");
        
        this._adapters[this._addedAdapters] = adapter;
        this._addedAdapters++;
    }

    public IEnumerable<NetworkAdapter> GetAdapters() => new List<NetworkAdapter>(this._adapters.Take(this._addedAdapters));

    public abstract PingClient GetPingClient();
}