using System;
using Joyersch.Monogame.Ui.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Ui.Text;

namespace NoNameButtonGame.Ui.Buttons;

public class TextButton<T> : IButton where T : IButton
{
    public BasicText Text { get; }

    public T Button;
    public Rectangle[] Hitbox => Button.Hitbox;
    public Rectangle Rectangle => Button.Rectangle;
    public event Action<object>? Leave;
    public event Action<object>? Enter;
    public event Action<object>? Click;

    public TextButton(string text, T button) : this(text, 1F, button)
    {
    }

    public TextButton(string text, float scale, T button)
    {
        Button = button;
        Button.Leave += _ => Leave?.Invoke(this);
        Button.Enter += _ => Enter?.Invoke(this);
        Button.Click += _ => Click?.Invoke(this);
        Text = new BasicText(text, scale * 2f);
        Text.InRectangle(Button)
            .OnCenter()
            .Centered()
            .Apply();
    }

    public float Layer
    {
        get => Button.Layer;
        set => Button.Layer = value;
    }

    public bool IsHover => Button.IsHover;

    public float Scale => Button.Scale;


    public virtual void Update(GameTime gameTime)
    {
        Button.Update(gameTime);
        Text.Update(gameTime);
        Text.InRectangle(Button)
            .OnCenter()
            .Centered()
            .Apply();
    }

    public bool UpdateInteraction(GameTime gameTime, IHitbox toCheck)
        => Button.UpdateInteraction(gameTime, toCheck);

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Button.Draw(spriteBatch);
        Text.Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => Button.GetPosition();

    public Vector2 GetSize()
        => Button.GetSize();

    public void Move(Vector2 newPosition)
    {
        Button.Move(newPosition);
        Text.InRectangle(Button)
            .OnCenter()
            .Centered()
            .Apply();
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
        => Button.ChangeColor(input);

    public int ColorLength()
        => Button.ColorLength();

    public Microsoft.Xna.Framework.Color[] GetColor()
        => Button.GetColor();

    public virtual void SetScale(ScaleProvider provider)
    {
        Button.SetScale(provider);
        Text.SetScale(provider);
    }
}