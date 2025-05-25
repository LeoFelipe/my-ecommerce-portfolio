using System.Diagnostics;

namespace EcommercePortfolio.IntegrationTests.Helpers.Configurations;

public static class DockerComposeHelper
{
    private readonly static string ComposeFile =
        Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.Parent!.FullName, "docker-compose.testing.yml");   

    public static async Task UpAsync()
    {
        await RunDockerCommand($"compose -f {ComposeFile} up -d --build");
    }

    public static async Task DownAsync()
    {
        await RunDockerCommand($"compose -f {ComposeFile} down --volumes");
    }

    private static async Task RunDockerCommand(string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "docker",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        process.OutputDataReceived += (s, e) => Console.WriteLine($"[docker stdout] {e.Data}");
        process.ErrorDataReceived += (s, e) => Console.WriteLine($"[docker stderr] {e.Data}");

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        Console.WriteLine($"[DEBUG] Docker process exited with code {process.ExitCode}");
    }
}
