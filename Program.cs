using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameLauncher.Util;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace GameLauncher
{
    internal static class Program
    {
        private static readonly Mutex _mutex = new(false, Path.GetFileName(Application.ExecutablePath));
        private static readonly string _logFile = "gamelauncher-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".log";
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
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                foreach (var parm in args)
                {
                    if (parm.IndexOf("/dev", StringComparison.OrdinalIgnoreCase) >= 0)
                        IsDeveloperMode = true;
                }

#if DEBUG
                // PracticeMedicine:
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
                catch (IOException)
                {
                }
#endif

                Console.SetOut(new MultiTextWriter(Console.Out, new ConsoleToFileWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _logFile))));
                LoggingUtil.Info("Program starting up...");

                if (!_mutex.WaitOne(0, false))
                {
                    HWND handle = PInvoke.FindWindow(null, "Game Launcher");
                    PInvoke.ShowWindow(handle, SHOW_WINDOW_CMD.SW_SHOW);
                    PInvoke.SetForegroundWindow(handle);
                    return 0;
                }
                LoggingUtil.Info("Checking for updates...");

                if (ReleaseUtil.CheckForUpdates().ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    if (MessageBox.Show("There's an update available! Do you want to download it?", "Game Launcher", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                    {
                        var folderBrowserDlg = new FolderBrowserDialog();
                        folderBrowserDlg.Description = "Select an folder where to download the latest \"*.zip\" of GameLauncher.";
                        folderBrowserDlg.RootFolder = Environment.SpecialFolder.Desktop;
                        folderBrowserDlg.ShowNewFolderButton = true;
                        if (folderBrowserDlg.ShowDialog() == DialogResult.OK)
                        {
                            if (string.IsNullOrEmpty(folderBrowserDlg.SelectedPath))
                                throw new NullReferenceException("\"folderBrowserDlg.SelectedPath\" returned null! Don't ask me why it's possible.");

                            if (!Directory.Exists(folderBrowserDlg.SelectedPath))
                                throw new DirectoryNotFoundException("The selected directory is invalid!");

                            ReleaseUtil.DownloadFileCompleted += (sender, e) =>
                            {
                                MessageBox.Show(
                                    string.Format("\"{0}\" has been downloaded into \"{1}\"!\n" +
                                    "Extract the archive using File Explorer or any program that can be associated with the \"*.zip\" type.",
                                    ReleaseUtil.ZipFileName, folderBrowserDlg.SelectedPath), "Game Launcher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Application.Exit();
                            };

                            ReleaseUtil.DownloadLatestRelease(folderBrowserDlg.SelectedPath + "/" + ReleaseUtil.ZipFileName)
                                .ConfigureAwait(false).GetAwaiter().GetResult();
                        }
                    }
                }
                else
                {
                    LoggingUtil.Info("Client up-to-date with the latest version.");

                    MessageBoxManager.Abort = "Scan";
                    MessageBoxManager.Retry =
                    MessageBoxManager.Cancel = "Browse";
                    MessageBoxManager.Register();
                    Application.Run(new Launcher());
                    MessageBoxManager.Unregister();
                }

                _mutex.ReleaseMutex();

                LoggingUtil.Info($"Goodbye, {Environment.UserName}!");
            }
            catch (Exception e)
            {
                var excBox = new ExceptionBox(e);
                LoggingUtil.Error($"FATAL ERROR: {e.Message}");
                excBox.ShowDialog();
            }

            return 0;
        }
    }
}
