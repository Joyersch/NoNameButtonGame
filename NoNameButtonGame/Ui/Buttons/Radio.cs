using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Ui.Buttons;

namespace Joyersch.Monogame.Ui.Buttons;

public sealed class Radio : TextButton<SquareButton>
{
    private bool _checked;

    public event Action<bool> ValueChanged;

    public static float DefaultScale { get; set; } = 4F;

    public Radio(RadioGroup group) : this(group, false)
    {
    }

    public Radio(RadioGroup group, float scale) : this(group, scale, false)
    {
    }

    public Radio(RadioGroup group, bool state) : this(group, DefaultScale, state)
    {
    }

    public Radio(RadioGroup group, float scale, bool state) : base(string.Empty, new SquareButton(Vector2.Zero, scale))
    {
        _checked = state;

        group.Register(this);

        Click += delegate
        {
            group.ResetAll();
            _checked = true;
            Button.SecondLayer = _checked;

            ValueChanged?.Invoke(_checked);
        };
        // Update Text
        Move(GetPosition());
    }

    public bool Checked()
        => _checked;

    public void Check()
        => Button.SecondLayer = _checked = true;

    public void Uncheck()
        => Button.SecondLayer = _checked = false;
}