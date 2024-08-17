using Microsoft.Xna.Framework;
using MonoUtils.Ui.Buttons;

namespace NoNameButtonGame.GameObjects.Buttons;

public sealed class Button : TextButton<SampleButton>
{
    public Button(string text, float scale = 8F, float textScale = 1F) : base(text, textScale,
        new SampleButton(Vector2.Zero, scale))
    {
    }
}