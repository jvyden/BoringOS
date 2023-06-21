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
}