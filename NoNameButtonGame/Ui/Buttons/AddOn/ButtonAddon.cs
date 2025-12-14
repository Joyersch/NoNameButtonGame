using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Ui.Buttons.AddOn;

public class ButtonAddon : IButton
{
    protected IButton Button;

    public Rectangle[] Hitbox => Button.Hitbox;
    public Rectangle Rectangle => Button.Rectangle;

    public float Layer
    {
        get => Button.Layer;
        set => Button.Layer = value;
    }

    public bool IsHover => Button.IsHover;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;


    public ButtonAddon(IButton button)
    {
        Button = button;
    }

    public virtual bool UpdateInteraction(GameTime gameTime, IHitbox toCheck)
        => Button.UpdateInteraction(gameTime, toCheck);

    public virtual void Update(GameTime gameTime)
    {
        Button.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Button.Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => Button.GetPosition();

    public Vector2 GetSize()
        => Button.GetSize();

    public virtual void Move(Vector2 newPosition)
        => Button.Move(newPosition);

    public virtual void ChangeColor(Microsoft.Xna.Framework.Color[] input)
        => Button.ChangeColor(input);

    public int ColorLength()
        => 1;

    public Microsoft.Xna.Framework.Color[] GetColor()
        => Button.GetColor();

    protected void InvokeLeave()
        => Leave?.Invoke(Button);

    protected void InvokeEnter()
        => Enter?.Invoke(Button);

    protected void InvokeClick()
        => Click?.Invoke(Button);

    public float Scale => Button.Scale;

    public virtual void SetScale(ScaleProvider provider)
        => Button.SetScale(provider);
}