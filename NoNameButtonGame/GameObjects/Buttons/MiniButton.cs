using Microsoft.Xna.Framework;
using MonoUtils.Ui.Buttons;

namespace NoNameButtonGame.GameObjects.Buttons;

public sealed class MiniButton(string text, string name) : TextButton<SelectButton>(text, new SelectButton(Vector2.Zero, 8F))
{
    public string Name { get; } = name;

    public MiniButton(string text) : this(text, string.Empty)
    {
    }
}