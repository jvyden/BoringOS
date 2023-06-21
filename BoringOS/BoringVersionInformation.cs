namespace BoringOS;

public static partial class BoringVersionInformation
{
    public const string Name = "BoringOS";
    private static partial string GetCommitHash();

    public static readonly string CommitHash = GetCommitHash();

    public const string Type =
#if DEBUG
        "Debug";
#else
        "Release";
#endif

    public static readonly string FullVersion = $"{Name} {Type} (commit {CommitHash})";
}