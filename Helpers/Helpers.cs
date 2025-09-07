using System;
using System.Drawing;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;

namespace GameLauncher
{
    public static class Helpers
    {
        public static void LazyUpdateLabel(Label label, string text)
        {
            if (label.Text != text)
                label.Text = text;
        }

        public static string FormatPlayTime(double time)
        {
            if (time == 0)
                return "Never";
            else if (time < 60)
                return "<1 minute";
            else if (time / 3600f < 1f)
            {
                var mins = (int)Math.Round(time / 60f, 0, MidpointRounding.ToEven);
                return $"{mins} {(mins == 1 ? "minute" : "minutes")}";
            }
            else
            {
                var hour = Math.Round(time / 3600f, 1, MidpointRounding.ToEven);
                return hour + (hour == 1 ? " hour" : " hours");
            }
        }

        public static Bitmap GetIcon(string path)
        {
            HICON hIcon = (HICON)SysCalls.GetJumboIcon(SysCalls.GetIconIndex(path));
            Icon icon = (Icon)Icon.FromHandle(hIcon).Clone();
            PInvoke.DestroyIcon(hIcon);
            return icon.ToBitmap();
        }
    }
}
