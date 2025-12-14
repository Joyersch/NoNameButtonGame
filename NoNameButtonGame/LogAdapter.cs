using Joyersch.Monogame.Console;
using Joyersch.Monogame.Logging;
using Joyersch.Monogame.Ui;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Ui;

namespace NoNameButtonGame;

public sealed class LogAdapter : ILogAdapter
{
    private DevConsole _console;
    public string LeftBracket => "[SBO]";
    public string RightBracket => "[SBC]";
    private int _line;

    public LogAdapter(DevConsole console)
    {
        _console = console;
    }

    public void SetLine(int line)
    {
        _line = line;
    }

    public void Write(string text)
    {
        _console.Write(text, _line);
    }

    public void WriteColor(string text, Color[] color)
    {
        _console.WriteColor(text, new BacklogColorSet(color));
    }

    public void WriteColor(string text, Color color)
    {
        _console.WriteColor(text, new BacklogColorSet(color, text.Length));
    }


    public void UpdateReference(DevConsole console)
    {
        _console = console;
    }
}