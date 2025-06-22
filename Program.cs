using GameLauncher.Util;
using KPreisser.UI;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace GameLauncher
{
    internal static class Program
    {
        private static readonly Mutex _mutex = new(false, Path.GetFileName(Application.ExecutablePath));
        private static readonly string _logFile = $"{DateTime.Now:dd-MM-yyyy-HH-mm-ss}.log";
        public static bool IsDeveloperMode { get; private set; } =
#if DEBUG
            true;
#else
            false;
#endif

        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs")))
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"));

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                foreach (var parm in args)
                {
                    if (parm.IndexOf("/dev", StringComparison.OrdinalIgnoreCase) >= 0)
                        IsDeveloperMode = true;
                }

#if DEBUG
                // @PracticeMedicine:
                // the output encoding differs based on whether the app is a Console (debug) or Windows app (release).
                // since because the flag also effects the default output encoding when reading from other processes'
                // standard output, we explicitly set the encoding to get consistent behavior in the debug and
                // rel builds of Game Launcher.
                // SharpDevelop also does this: https://github.com/icsharpcode/SharpDevelop/blob/master/src/Main/SharpDevelop/Startup/SharpDevelopMain.cs#L150
                //
                // we need to wrap this in a try and catch block, because it will throw an IOException
                // when Game Launcher doesn't have a console window.
                try
                {
                    Console.OutputEncoding = System.Text.Encoding.Default;
                    LoggingUtil.Debug("Output encoding set to default.");
                }
                catch (IOException) {}
#endif
                Console.SetOut(new MultiTextWriter(Console.Out, new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", _logFile), append: true) { AutoFlush = true }));
                LoggingUtil.Info("Starting up...");

                if (!_mutex.WaitOne(0, false))
                {
                    HWND handle = PInvoke.FindWindow(null, "GameLauncher");
                    PInvoke.ShowWindow(handle, SHOW_WINDOW_CMD.SW_SHOW);
                    PInvoke.SetForegroundWindow(handle);
                    return 0;
                }

                LoggingUtil.Info("Checking for updates...");
                
                Version dismissedVersion;
                using (FileStream stream = File.Open($@"{Environment.CurrentDirectory}\config.json", System.IO.FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                using (var reader = new StreamReader(stream))
                {
                    var raw = reader.ReadToEnd();
                    var config = JsonConvert.DeserializeObject<Config>(raw);

                    if (config.DismissedUpdate == null)
                        dismissedVersion = new("0.0");
                    else
                        dismissedVersion = new(config.DismissedUpdate);
                }

                if (ReleaseUtil.CheckForUpdates().ConfigureAwait(false).GetAwaiter().GetResult() && dismissedVersion < ReleaseUtil.LatestVersion)
                {
                    TaskDialog updateDialog = new(new()
                    {
                        Title = "GameLauncher",
                        Instruction = "A new update is available!",
                        Text = "Do you want to download it?",
                        CustomButtons = [new TaskDialogCustomButton("Yes"), new TaskDialogCustomButton("No"), new TaskDialogCustomButton("Dismiss till next update")],
                        Expander = new() {Text = $"You are running v{ReleaseUtil.CurrentVersion}. The latest version is <a href=\"https://github.com/y2k04/gamelauncher/releases/tag/v{ReleaseUtil.LatestVersion}\">v{ReleaseUtil.LatestVersion}</a>.", ExpandFooterArea = true},
                        Icon = TaskDialogStandardIcon.Information,
                        EnableHyperlinks = true
                    });
                    updateDialog.Page.HyperlinkClicked += (_, e) => Process.Start(e.Hyperlink);

                    TaskDialogButton result = updateDialog.Show();
                    if (result == updateDialog.Page.CustomButtons[0])
                    {
                        var folderBrowserDlg = new FolderBrowserDialog
                        {
                            Description = "Select an folder where to download the latest \"*.zip\" of GameLauncher.",
                            RootFolder = Environment.SpecialFolder.Desktop,
                            ShowNewFolderButton = true
                        };
                        if (folderBrowserDlg.ShowDialog() == DialogResult.OK)
                        {
                            if (string.IsNullOrEmpty(folderBrowserDlg.SelectedPath))
                                throw new NullReferenceException("\"folderBrowserDlg.SelectedPath\" returned null! Don't ask me why it's possible.");

                            if (!Directory.Exists(folderBrowserDlg.SelectedPath))
                                throw new DirectoryNotFoundException("The selected directory is invalid!");

                            TaskDialog downloadProgress = new(new()
                            {
                                Title = "GameLauncher",
                                Instruction = $"Downloading v{ReleaseUtil.LatestVersion}...",
                                Text = "0% complete",
                                ProgressBar = new()
                            });

                            ReleaseUtil.DownloadFileCompleted += (sender, e) =>
                            {
                                downloadProgress.Close();
                                new TaskDialog(new()
                                {
                                    Title = "GameLauncher",
                                    Instruction = $"Version {ReleaseUtil.LatestVersion} has been downloaded!",
                                    Text = $"Extract the archive using File Explorer or any program that can be associated with the \"*.zip\" type.\n\nIt is located at: \"{folderBrowserDlg.SelectedPath}\".",
                                    StandardButtons = [new TaskDialogStandardButton(TaskDialogResult.OK)],
                                    Icon = TaskDialogStandardIcon.SecuritySuccessGreenBar
                                }).Show();
                                Application.Exit();
                            };

                            ReleaseUtil.DownloadProgressChanged += (s, e) =>
                            {
                                var p = e.ProgressPercentage;
                                downloadProgress.SetProgressBarPosition(p);
                                downloadProgress.Page.Text = $"{p}% complete";
                            };

                            downloadProgress.Shown += (s, e) => ReleaseUtil.DownloadLatestRelease($"{folderBrowserDlg.SelectedPath}/{ReleaseUtil.ZipFileName}").ConfigureAwait(false).GetAwaiter().GetResult();
                            downloadProgress.Show();
                        }
                    } else if (result == updateDialog.Page.CustomButtons[1])
                    {
                        LoggingUtil.Info("Update cancelled. Will ask again on next launch.");
                        Application.Run(new Launcher());
                    }
                    else if (result == updateDialog.Page.CustomButtons[2])
                    {
                        LoggingUtil.Info($"Ignoring v{ReleaseUtil.LatestVersion} until next release.");
                        Application.Run(new Launcher(ReleaseUtil.LatestVersion.ToString()));
                    }
                }
                else
                {
                    LoggingUtil.Info("Client up-to-date with the latest version.");
                    Application.Run(new Launcher());
                }

                
                _mutex.ReleaseMutex();
                LoggingUtil.Info($"Goodbye!");
            }
            catch (Exception e)
            {
                LoggingUtil.Error($"FATAL ERROR: {e.Message}");
                new ExceptionBox(e).ShowDialog();
            }

            return 0;
        }
    }
}
