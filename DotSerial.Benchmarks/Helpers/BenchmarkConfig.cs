using System.Diagnostics;
using BenchmarkDotNet.Configs;

internal class BenchmarkConfig
{
    internal static ManualConfig Create()
    {
        var folderName = $"{GetSanitizedBranchName()}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}";

        var config = DefaultConfig.Instance.WithArtifactsPath(Path.Combine("BenchmarkDotNet.Artifacts", folderName));

        return config;
    }

    private static string GetGitBranchName()
    {
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "rev-parse --abbrev-ref HEAD",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };

            process.Start();
            var branch = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            return string.IsNullOrEmpty(branch) ? "unknown" : branch;
        }
        catch (Exception)
        {
            return "unknown";
        }
    }

    private static string GetSanitizedBranchName()
    {
        var branch = GetGitBranchName();

        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            branch = branch.Replace(invalidChar, '-');
        }

        return branch;
    }
}
