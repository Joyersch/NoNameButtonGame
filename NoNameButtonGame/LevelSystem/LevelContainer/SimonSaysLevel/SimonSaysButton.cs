using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoUtils.Logic;
using MonoUtils.Sound;
using MonoUtils.Ui.Objects.Buttons;

namespace NoNameButtonGame.LevelSystem.LevelContainer.SimonSaysLevel;

public class SimonSaysButton : TextButton<SampleButton>
{
    private readonly OverTimeInvoker _invoker;
    private readonly Color _color;
    private readonly Color _hightlight;
    private readonly EffectsRegistry _effects;
    private readonly Keys _key;
    public Color Color => _color;

    public event Action Finished;

    public enum Keys
    {
        note_c,
        note_d,
        note_e,
        note_f,
        note_g
    }

    public SimonSaysButton(Color color, Color highlight, float time, EffectsRegistry effects, Keys key) : this(Vector2.Zero, "[block]", 1F, color,
        highlight, time, effects, key)
    {
    }

    public SimonSaysButton(Vector2 position, string text, float scale, Color color, Color highlight, float time, EffectsRegistry effects, Keys key) : base(
        text, scale, new SampleButton(position))
    {
        _color = color;
        _hightlight = highlight;
        _effects = effects;
        _key = key;
        _invoker = new OverTimeInvoker(time, false)
        {
            InvokeOnce = true
        };
        _invoker.Trigger += InvokerTrigger;
        ResetColor();
    }

    private void InvokerTrigger()
    {
        ResetColor();
        Finished?.Invoke();
    }

    public void Highlight()
    {
        _invoker.Start();
        Text.ChangeColor(_hightlight);
        var effect = _effects.GetInstance(_key.ToString());
        effect?.Play();
    }

    private void ResetColor()
    {
        Text.ChangeColor(_color);
    }

    public override void Update(GameTime gameTime)
    {
        _invoker.Update(gameTime);
        base.Update(gameTime);
    }
}