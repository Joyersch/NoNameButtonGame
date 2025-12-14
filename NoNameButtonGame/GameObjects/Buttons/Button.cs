using Joyersch.Monogame.Ui.Buttons;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Ui.Buttons;

namespace NoNameButtonGame.GameObjects.Buttons;

public sealed class Button : TextButton<SampleButton>
{
    private static readonly float DefaultScale = 8F;
    private static readonly float DefaultTextScale = 2F;

    public Button(string text) : this(text, DefaultScale, DefaultTextScale)
    {
    }
    
    public Button(string text, float scale) : this(text, scale, DefaultTextScale)
    {
    }

    public Button(string text, float scale, float textScale) : base(text, textScale,
        new SampleButton(Vector2.Zero, scale))
    {
    }
}