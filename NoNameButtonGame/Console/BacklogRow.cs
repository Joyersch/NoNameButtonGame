namespace Joyersch.Monogame.Console;

public sealed class BacklogRow
{
    public string Text { get; private set; }
    public BacklogColorSet ColorSet { get; private set; }

    public BacklogRow(string text)
    {
        Text = text;
        ColorSet = new BacklogColorSet(text.Length);
    }

    public BacklogRow(string text, BacklogColorSet colorSet)
    {
        Text = text;
        ColorSet = colorSet;
    }

    public void SetText(string text)
        => Text = text;

    public void SetColor(BacklogColorSet color)
        => ColorSet = color;
}