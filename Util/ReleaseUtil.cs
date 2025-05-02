using System;
using Octokit;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;

namespace GameLauncher.Util;

/// <summary>
/// Manages the you already know.
/// </summary>
public class ReleaseUtil
{
    private string _curVersion = "v" + Assembly.GetEntryAssembly().GetName().Version.ToString();
    private string _zipFile = "GameLauncher-{0}.zip";
    private const string RepoOrg = "y2k04";
    private const string RepoUri = "gamelauncher";

    public DownloadDataCompletedEventHandler DownloadDataCompleted { get; set; }
    public DownloadProgressChangedEventHandler DownloadProgressChanged { get; set; }

    /// <summary>
    /// Gets all of the information about the releases and compares them to the current version of the executable.
    /// </summary>
    /// <returns>Returns true when the latest version is higher than the current.</returns>
    public async Task<bool> CheckForUpdates()
    {
        if (_curVersion == null) return false;

        try
        {
            var ghClient = new GitHubClient(new ProductHeaderValue("GameLauncher", _curVersion));
            var releases = await ghClient.Repository.Release.GetAll(RepoOrg, RepoUri);
            var latestRel = releases[0];

            if (latestRel != null)
            {
                // Parse versions for comparison  
                var currentVersion = new Version(_curVersion);
                var latestVersion = new Version(latestRel.TagName);

                return currentVersion < latestVersion;
            }
        }
        catch (Exception)
        {
            // eh it will return false anyways.
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task DownloadLatestRelease(string filePath)
    {
        try
        {
            var ghClient = new GitHubClient(new ProductHeaderValue("GameLauncher", _curVersion));
            var releases = await ghClient.Repository.Release.GetAll(RepoOrg, RepoUri);
            var latestRel = releases[0];

            if (latestRel != null)
            {
                var relAssets = latestRel.Assets;
                foreach (var asset in relAssets)
                {
                    // if the exact archive file is found, download it.
                    if (asset.Name == string.Format(_zipFile, latestRel.TagName))
                    {
                        using var wc = new WebClient();
                        if (DownloadDataCompleted != null) wc.DownloadDataCompleted += DownloadDataCompleted;
                        if (DownloadProgressChanged != null) wc.DownloadProgressChanged += DownloadProgressChanged;
                        wc.DownloadFileAsync(new Uri(asset.BrowserDownloadUrl), filePath);
                        break;
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
}
