using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame;
using NoNameButtonGame.Ui.Buttons;

namespace Joyersch.Monogame.Ui.Buttons;

public sealed class Checkbox : TextButton<SquareButton>
{
    private bool _checked;

    public event Action<bool> ValueChanged;

    private readonly static float DefaultScale = 4F;

    public Microsoft.Xna.Framework.Color CheckedColor { get; set; } = Microsoft.Xna.Framework.Color.Green;
    public Microsoft.Xna.Framework.Color UncheckedColor { get; set; } = Microsoft.Xna.Framework.Color.Red;

    private Microsoft.Xna.Framework.Color TextColor => _checked ? CheckedColor : UncheckedColor;

    public string CheckedText { get; set; } = "[checkmark]";
    public string UncheckedText { get; set; } = "[crossout]";

    private string TextValue => _checked ? CheckedText : UncheckedText;

    public bool UseTexture { get; set; } = false;

    public Checkbox() : this(1f)
    {
    }
    
    public Checkbox(bool state) : this(DefaultScale, state:state)
    {
    }
    
    public Checkbox(float scale, float textScale = 1f, bool state = false) : base(string.Empty, textScale, new SquareButton(Vector2.Zero, scale))
    {
        _checked = state;
        if (!UseTexture)
        {
            Text.ChangeText(TextValue);
            Text.ChangeColor([TextColor]);
        }

        Click += delegate
        {
            _checked = !_checked;
            if (UseTexture)
                Button.SecondLayer = _checked;
            else
            {
                Text.ChangeText(TextValue);
                Text.ChangeColor([TextColor]);
            }

            ValueChanged?.Invoke(_checked);
        };
        // Update Text
        Move(GetPosition());
    }

    public bool Checked()
        => _checked;

    public bool Check()
        => _checked = true;

    public bool Unchecked()
        => _checked = false;

    public override void SetScale(ScaleProvider provider)
    {
        base.SetScale(provider);
    }
}