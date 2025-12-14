using Joyersch.Monogame.Ui.Buttons;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Ui.Buttons;

public sealed class SquareTextButton : TextButton<SquareButton>
{
    private static readonly float DefaultScale = 4f;
    public SquareTextButton(string text) : this(text, Vector2.Zero)
    {
    }

    public SquareTextButton(string text, Vector2 position) : this(text, position, DefaultScale)
    {
    }

    public SquareTextButton(string text, Vector2 position, float scale) : base(text, new SquareButton(position, scale))
    {
    }
}