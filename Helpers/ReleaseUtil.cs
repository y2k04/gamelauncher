using System;
using Octokit;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;
using System.ComponentModel;

namespace GameLauncher.Util;

/// <summary>
/// Manages the you already know.
/// </summary>
public static class ReleaseUtil
{
    private static string _curVersion = "v" + Assembly.GetEntryAssembly().GetName().Version.ToString();
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
        if (_curVersion == null)
        {
            LoggingUtil.Warn("_curVersion is null!"); 
            return false;
        }

        try
        {
            var ghClient = new GitHubClient(new ProductHeaderValue("GameLauncher", _curVersion));
            var releases = await ghClient.Repository.Release.GetAll(RepoOrg, RepoName);
            var latestRel = releases[0];

            if (latestRel != null)
            {
                // Parse versions for comparison  
                var currentVersion = new Version(_curVersion.TrimStart('v').TrimEnd());
                var latestVersion = new Version(latestRel.TagName.TrimStart('v').TrimEnd());

                LoggingUtil.Info($"Comparing client version {currentVersion.ToString()} to latest version from remote {latestVersion.ToString()}");
                _zipFile = string.Format(_zipFile, latestRel.TagName);

                return currentVersion < latestVersion;
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

            var ghClient = new GitHubClient(new ProductHeaderValue("GameLauncher", _curVersion));
            var releases = await ghClient.Repository.Release.GetAll(RepoOrg, RepoName);
            var latestRel = releases[0];

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
