using System.Collections.Immutable;

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

    public const int MaxAdapters = 4; // TODO: support multiple adapters
    private NetworkAdapter? _adapter;

    protected void AddAdapter(NetworkAdapter adapter)
    {
        this._adapter = adapter;
    }

    public IEnumerable<NetworkAdapter> GetAdapters()
    {
        if(this._adapter == null) return new List<NetworkAdapter>(0);
        return new List<NetworkAdapter>(1) { this._adapter };
    }
}