namespace BoringOS;

public interface IBoringKernel
{
    public bool HaltKernel();
    public int CollectGarbage();
}