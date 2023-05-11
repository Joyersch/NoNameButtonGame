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

    private bool _triggered;

    public event Action Finished;
    public SimonSaysButton(Color color) : this(Vector2.Zero, DefaultSize * 2, string.Empty, Letter.Full.ToString()
        , DefaultTextSize * 2, 1,
        DefaultTexture, DefaultMapping, color)
    {
    }

    public SimonSaysButton(Vector2 position, Vector2 size, string name, string text, Vector2 textSize, int spacing,
        Texture2D texture, TextureHitboxMapping mapping, Color color) : base(position, size, name, text, textSize,
        spacing, texture,
        mapping)
    {
        _color = color;
        _invoker = new OverTimeInvoker(0F);
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

    public bool Highlight(Color color, float time)
    {
        var wasSuccessful = _invoker.ChangeTime(time);
        if (!wasSuccessful)
            return false;
        _invoker.Start();
        Text.ChangeColor(color);
        _triggered = true;
        return true;
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