// Services/WingetManager.cs
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LaptopSupport;
using Microsoft.Extensions.Logging;

namespace LaptopSupport.Services
{
    // Manages operations using the Windows Package Manager (WinGet).
    public class WingetManager
    {
        private readonly ILogger<WingetManager> _logger;

        public WingetManager(ILogger<WingetManager> logger)
        {
            _logger = logger;
        }

        // Asynchronously installs a list of applications using WinGet.
        public async IAsyncEnumerable<ProgressUpdate> InstallAppsAsync(IEnumerable<string> appIds, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            int totalApps = new List<string>(appIds).Count;
            int appsCompleted = 0;

            foreach (var appId in appIds)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield return new ProgressUpdate { CurrentTask = "Installation cancelled.", Status = ProgressUpdate.Types.Status.Failed };
                    yield break;
                }

                yield return new ProgressUpdate
                {
                    CurrentTask = $"Starting installation of {appId}...",
                    OverallPercentage = (int)(((double)appsCompleted / totalApps) * 100),
                    Status = ProgressUpdate.Types.Status.InProgress
                };

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "winget",
                    Arguments = $"install --id {appId} --accept-package-agreements --accept-source-agreements --silent",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using var process = new Process { StartInfo = processStartInfo };
                process.Start();

                // We can stream the output for more granular progress, but for now, we wait.
                await process.WaitForExitAsync(cancellationToken);

                if (process.ExitCode == 0)
                {
                    appsCompleted++;
                    yield return new ProgressUpdate
                    {
                        CurrentTask = $"✅ Successfully installed {appId}.",
                        OverallPercentage = (int)(((double)appsCompleted / totalApps) * 100),
                        Status = ProgressUpdate.Types.Status.InProgress
                    };
                }
                else
                {
                    var errorOutput = await process.StandardError.ReadToEndAsync();
                    _logger.LogError("Failed to install {AppId}. Exit Code: {ExitCode}. Error: {Error}", appId, process.ExitCode, errorOutput);
                    yield return new ProgressUpdate
                    {
                        CurrentTask = $"❌ Failed to install {appId}. Error: {errorOutput}",
                        OverallPercentage = (int)(((double)appsCompleted / totalApps) * 100),
                        Status = ProgressUpdate.Types.Status.Failed
                    };
                    // Stop on first failure
                    yield break;
                }
            }

            yield return new ProgressUpdate { CurrentTask = "All installations completed successfully.", OverallPercentage = 100, Status = ProgressUpdate.Types.Status.Completed };
        }
    }
}
