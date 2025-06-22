using Octokit;
using System;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace GameLauncher.Util;

/// <summary>
/// Manages the you already know.
/// </summary>
public static class ReleaseUtil
{
    public static Version CurrentVersion = Assembly.GetEntryAssembly().GetName().Version;
    public static Version LatestVersion = new("1.0.0");
    private static string _zipFile = "GameLauncher-{0}.zip";
    public static string ZipFileName { get => _zipFile; }
    private const string RepoOrg = "y2k04";
    private const string RepoName = "gamelauncher";

    public static DownloadDataCompletedEventHandler DownloadDataCompleted { get; set; }
    public static AsyncCompletedEventHandler DownloadFileCompleted { get; set; }
    public static DownloadProgressChangedEventHandler DownloadProgressChanged { get; set; }

    /// <summary>
    /// Gets all of the information about the releases and compares them to the current version of the executable.
    /// </summary>
    /// <returns>Returns true when the latest version is higher than the current.</returns>
    public static async Task<bool> CheckForUpdates()
    {
        if (CurrentVersion == null)
        {
            LoggingUtil.Warn("I don't know how, but CurrentVersion is null..."); 
            return false;
        }

        try
        {
            var ghClient = new GitHubClient(new ProductHeaderValue("GameLauncher", $"v{CurrentVersion}"));
            var latestRel = await ghClient.Repository.Release.GetLatest(RepoOrg, RepoName);

            if (latestRel != null)
            {
                // Parse versions for comparison
                LatestVersion = new Version(latestRel.TagName.TrimStart('v').TrimEnd());

                LoggingUtil.Info($"Comparing client version {CurrentVersion} to latest version from remote {LatestVersion}");
                _zipFile = string.Format(_zipFile, latestRel.TagName);

                return CurrentVersion < LatestVersion;
            }

            LoggingUtil.Warn("latestRel is null!");
        }
        catch (Exception e)
        {
            // eh it will return false anyways.
            LoggingUtil.Warn($"Failed to check for updates: {e.Message}\nStack Trace:\n{e.StackTrace}");
        }

        return false;
    }

    /// <summary>
    /// Downloads the latest release to the specified file path.
    /// </summary>
    /// <param name="filePath">Output path</param>
    public static async Task DownloadLatestRelease(string filePath)
    {
        try
        {
            if (!await CheckForUpdates()) return;

            var ghClient = new GitHubClient(new ProductHeaderValue("GameLauncher", $"v{CurrentVersion}"));
            var latestRel = await ghClient.Repository.Release.GetLatest(RepoOrg, RepoName);

            if (latestRel != null)
            {
                var relAssets = latestRel.Assets;
                foreach (var asset in relAssets)
                {
                    // if the exact archive file is found, download it.
                    if (asset.Name == string.Format(_zipFile, latestRel.TagName))
                    {
                        LoggingUtil.Info("Found the asset! Downloading...");
                        using var wc = new WebClient();
                        if (DownloadDataCompleted != null) wc.DownloadDataCompleted += DownloadDataCompleted;
                        if (DownloadFileCompleted != null) wc.DownloadFileCompleted += DownloadFileCompleted;
                        if (DownloadProgressChanged != null) wc.DownloadProgressChanged += DownloadProgressChanged;
                        await wc.DownloadFileTaskAsync(new Uri(asset.BrowserDownloadUrl, UriKind.Absolute), filePath);
                        return;
                    }
                }

                LoggingUtil.Warn($"Latest release doesn't have the expected asset! (\"{string.Format(_zipFile, latestRel.TagName)}\")");
            }
        }
        catch (Exception e)
        {
            LoggingUtil.Error($"Error downloading latest asset: {e.Message}");
        }
    }
}
