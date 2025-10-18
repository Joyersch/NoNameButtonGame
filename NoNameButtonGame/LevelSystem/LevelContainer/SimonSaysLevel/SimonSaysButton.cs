using System;
using Joyersch.Monogame;
using Joyersch.Monogame.Sound;
using Joyersch.Monogame.Ui.Buttons;
using Joyersch.Monogame.Ui.Text;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects.Buttons;

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
        C,
        D,
        E,
        F,
        G
    }

    public SimonSaysButton(Color color, Color highlight, float time, EffectsRegistry effects, Keys key) : this(
        "[block]", color, highlight, time, effects, key)
    {
    }

    public SimonSaysButton(string text, Color color, Color highlight, float time, EffectsRegistry effects,
        Keys key) : base(text, 2f, new SampleButton(Vector2.Zero, 8f))
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
        var effect = _effects.GetInstance($"{Statics.Sfx.Notes.Base}{_key}");
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