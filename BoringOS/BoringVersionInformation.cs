namespace BoringOS;

public static partial class BoringVersionInformation
{
    private static partial string GetCommitHash();

    public static readonly string CommitHash = GetCommitHash();

    public const string Type =
#if DEBUG
        "Debug";
#else
        "Release";
#endif
}