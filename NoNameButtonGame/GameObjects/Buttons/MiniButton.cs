using Joyersch.Monogame.Ui.Buttons;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.GameObjects.Buttons;

public sealed class MiniButton(string text, string name)
    : TextButton<SelectButton>(text, 2f, new SelectButton(Vector2.Zero, DefaultScale))
{
    private static readonly float DefaultScale = 8F;
    public string Name { get; } = name;

    public MiniButton(string text) : this(text, string.Empty)
    {
    }
}