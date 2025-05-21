using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace GameLauncher;

internal class MultiTextWriter(params TextWriter[] writers) : TextWriter
{
    private readonly List<TextWriter> writers = [.. writers];
    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(string value)
    {
        foreach (var writer in writers)
            writer.Write(value);
    }

    public override void Write(char value)
    {
        foreach (var writer in writers)
            writer.Write(value);
    }

    public override void Write(bool value)
    {
        foreach (var writer in writers)
            writer.Write(value);
    }

    public override void Write(object value)
    {
        foreach (var writer in writers)
            writer.Write(value);
    }

    public override void WriteLine(string value)
    {
        foreach (var writer in writers)
            writer.WriteLine(value);
    }

    public override void WriteLine(char value)
    {
        foreach (var writer in writers)
            writer.WriteLine(value);
    }

    public override void WriteLine(bool value)
    {
        foreach (var writer in writers)
            writer.WriteLine(value);
    }

    public override void WriteLine(object value)
    {
        foreach (var writer in writers)
            writer.WriteLine(value);
    }

    public override void Flush()
    {
        foreach (var writer in writers)
            writer.Flush();
        base.Flush();
    }
}
