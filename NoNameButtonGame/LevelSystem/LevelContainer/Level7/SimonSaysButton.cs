using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.LogicObjects;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class SimonSaysButton : TextButton
{
    private readonly OverTimeInvoker _invoker;
    private readonly Color _color;
    private readonly Color _hightlight;
    public Color Color => _color;

    private bool _triggered;

    public event Action Finished;

    public SimonSaysButton(Color color, Color highlight, float time) : this(Vector2.Zero, DefaultSize * 2, string.Empty,
        Letter.Full.ToString(), DefaultTextSize * 2, 1, DefaultTexture, DefaultMapping, color, highlight, time)
    {
    }

    public SimonSaysButton(Vector2 position, Vector2 size, string name, string text, Vector2 textSize, int spacing,
        Texture2D texture, TextureHitboxMapping mapping, Color color, Color highlight, float time) : base(position, size, name,
        text, textSize, spacing, texture, mapping)
    {
        _color = color;
        _hightlight = highlight;
        _invoker = new OverTimeInvoker(time, false);
        _invoker.Trigger += ResetColor;
        _invoker.Trigger += InvokerTrigger;
        ResetColor();
    }

    private void InvokerTrigger()
    {
        if (!_triggered)
            return;
        _triggered = false;
        Finished?.Invoke();
    }

    public void Highlight()
    {
        _invoker.Start();
        Text.ChangeColor(_hightlight);
        _triggered = true;
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