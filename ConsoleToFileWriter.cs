using System;
using System.Text;
using System.IO;

namespace GameLauncher;

internal class ConsoleToFileWriter(string filePath) : TextWriter
{
    private readonly StreamWriter _fileWriter = new(filePath, append: true) { AutoFlush = true };

    public override void Write(string value)
    {
        _fileWriter.Write(value);
    }

    public override void Write(char value)
    {
        _fileWriter.Write(value);
    }

    public override void Write(bool value)
    {
        _fileWriter.Write(value);
    }

    public override void Write(object value)
    {
        _fileWriter.Write(value);
    }

    public override void WriteLine(string value)
    {
        _fileWriter.WriteLine(value);
    }

    public override void WriteLine(char value)
    {
        _fileWriter.WriteLine(value);
    }

    public override void WriteLine(bool value)
    {
        _fileWriter.WriteLine(value);
    }

    public override void WriteLine(object value)
    {
        _fileWriter.WriteLine(value);
    }

    public override void Flush()
    {
        _fileWriter.Flush();
        base.Flush();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _fileWriter.Dispose();
        }
        base.Dispose(disposing);
    }

    public override Encoding Encoding => Encoding.UTF8;
}
