namespace BoringOS.Time;

/// <summary>
/// A timer, similar to a <see cref="System.Diagnostics.Stopwatch"/>.
/// </summary>
public abstract class KernelTimer : IDisposable
{
    protected long StartTicks;

    public virtual void Start()
    {
        this.StartTicks = Now;
    }

    protected abstract long Now { get; }
    
    public long ElapsedTicks => Now - this.StartTicks;
    public long ElapsedMilliseconds => (long)(this.ElapsedTicks / 10_000.0);

#pragma warning disable CA1816
    public virtual void Dispose()
#pragma warning restore CA1816
    {
        
    }
}