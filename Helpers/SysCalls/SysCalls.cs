using System.Runtime.InteropServices;
using System;
using Windows.Win32;
using Windows.Win32.UI.Controls;

public static partial class SysCalls
{
    const int SHIL_JUMBO = 0x4;

    [DllImport("Shell32.dll")]
    public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

    public static int GetIconIndex(string pszFile)
    {
        SHFILEINFO sfi = new SHFILEINFO();
        SHGetFileInfo(pszFile, 0, ref sfi, (uint)Marshal.SizeOf(sfi), (uint)(SHGFI.SysIconIndex | SHGFI.LargeIcon | SHGFI.UseFileAttributes));
        return sfi.iIcon;
    }

    // 256*256
    public static IntPtr GetJumboIcon(int iImage)
    {
        Guid guil = new Guid("192B9D83-50FC-457B-90A0-2B82A8B5DAE1");

        PInvoke.SHGetImageList(SHIL_JUMBO, in guil, out object spiml);
        IntPtr hIcon = IntPtr.Zero;
        ((IImageList)spiml).GetIcon(iImage, (int)(IMAGE_LIST_DRAW_STYLE.ILD_TRANSPARENT | IMAGE_LIST_DRAW_STYLE.ILD_IMAGE), ref hIcon);

        return hIcon;
    }
}