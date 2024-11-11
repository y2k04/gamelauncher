using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace GameLauncher
{
    static class Program
    {
        static readonly Mutex _mutex = new Mutex(false, Path.GetFileName(Application.ExecutablePath));

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!_mutex.WaitOne(0, false))
            {
                HWND handle = PInvoke.FindWindow(null, "Game Launcher");
                PInvoke.ShowWindow(handle, SHOW_WINDOW_CMD.SW_SHOW);
                PInvoke.SetForegroundWindow(handle);
                return;
            }
            else
            {
                MessageBoxManager.Abort = "Scan";
                MessageBoxManager.Retry = 
                    MessageBoxManager.Cancel = "Browse";
                MessageBoxManager.Register();
                Application.Run(new Launcher());
            }
            _mutex.ReleaseMutex();
        }
    }
}
