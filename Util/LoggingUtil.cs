using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace GameLauncher.Util;

// XML documentation stolen from my low-level routine library. :/

/// <summary>
/// Ah, yes. The so-called "Chromium" logger in ".NET".
/// <para>
/// This kind of replicates Chromium's logging format + some of the code were just copied from the logger code of Chromium. (just some C++ keywords were changed)
/// </para>
/// </summary>
public static class LoggingUtil
{
    // yeah ik, this is dumb
    private static void SetColorFromLevel(string level)
    {
        level = level.ToLower().Trim(' ');
        switch(level)
        {
            case "warn":
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case "error":
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case "debug":
                Console.ForegroundColor = ConsoleColor.Blue;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
        }
    }

    private static string FormatMessage(string level, string message, string filePath, int ln)
    {
        var fileName = Path.GetFileName(filePath);
        var curDate = DateTime.Now.ToString("ddMMyy");
        var curTime = DateTime.Now.ToString("hhmmss");
        var builder = new StringBuilder();
        builder.Append("[");
        builder.Append(Process.GetCurrentProcess().Id);
        builder.Append(":");
        builder.Append(Environment.CurrentManagedThreadId);
        builder.Append(":");
        builder.Append(curDate);
        builder.Append(".");
        builder.Append(curTime);
        builder.Append(":");
        builder.Append(level);
        builder.Append(":");
        builder.Append(fileName);
        builder.Append(":");
        builder.Append(ln);
        builder.Append("] ");
        builder.Append(message);

        return builder.ToString();
    }

    public static void Info(string message, 
        [CallerFilePath] string filePath = null,
        [CallerLineNumber] int ln = 0)
    {
        SetColorFromLevel("INFO");
        Console.WriteLine(FormatMessage("INFO", message, filePath, ln));
        Console.ResetColor();
    }

    public static void Warn(string message, 
        [CallerFilePath] string filePath = null,
        [CallerLineNumber] int ln = 0)
    {
        SetColorFromLevel("WARN");
        Console.WriteLine(FormatMessage("WARN", message, filePath, ln));
        Console.ResetColor();
    }

    public static void Error(string message,
           [CallerFilePath] string filePath = null,
           [CallerLineNumber] int ln = 0)
    {
        SetColorFromLevel("ERROR");
        Console.WriteLine(FormatMessage("ERROR", message, filePath, ln));
        Console.ResetColor();
    }

    public static void Debug(string message,
           [CallerFilePath] string filePath = null,
           [CallerLineNumber] int ln = 0)
    {
        if (!Program.IsDeveloperMode)
            return;

        SetColorFromLevel("DEBUG");
        Console.WriteLine(FormatMessage("DEBUG", message, filePath, ln));
        Console.ResetColor();
    }
}
