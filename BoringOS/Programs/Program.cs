namespace BoringOS.Programs;

public abstract class Program
{
    protected Program(string name, string? description = null)
    {
        this.Name = name;
        this.Description = description ?? "No description available.";
    }

    public string Name { get; private init; }
    public string Description { get; private init; }

    public abstract byte Invoke(string[] args, BoringSession session);
}