using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level3;

public class SimonSaysButton : TextButton<SampleButton>
{
    private readonly OverTimeInvoker _invoker;
    private readonly Color _color;
    private readonly Color _hightlight;
    public Color Color => _color;

    public event Action Finished;

    public SimonSaysButton(Color color, Color highlight, float time) : this(Vector2.Zero, "[block]", 1F, color,
        highlight, time)
    {
    }

    public SimonSaysButton(Vector2 position, string text, float scale, Color color, Color highlight, float time) : base(
        text, scale, new SampleButton(position))
    {
        _color = color;
        _hightlight = highlight;
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
    }

    private void ResetColor()
    {
        Text.ChangeColor(_color);
    }

    public void Update(GameTime gameTime)
    {
        _invoker.Update(gameTime);
        base.Update(gameTime);
    }
}