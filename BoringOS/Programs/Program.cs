using JetBrains.Annotations;

namespace BoringOS.Programs;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class Program
{
    protected Program(string name, string? description = null)
    {
        this.Name = name;
        this.Description = description ?? "No description available.";
    }

    public string Name { get; private set; }
    public string Description { get; private set; }

    public abstract byte Invoke(string[] args, BoringSession session);
}