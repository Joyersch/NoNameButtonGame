using Microsoft.Xna.Framework;
using MonoUtils.Ui.Buttons;

namespace NoNameButtonGame.GameObjects.Buttons;

public class MiniButton : TextButton<SelectButton>
{
    public string Name { get; }

    public MiniButton(string text) : this(text, string.Empty)
    {
    }

    public MiniButton(string text, string name) : base(text, new SelectButton(Vector2.Zero))
    {
        Name = name;
    }
}