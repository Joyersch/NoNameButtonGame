using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.Buttons;

public class EmptyButton : GameObject, IMouseActions, IMoveable, IInteractable
{
    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;
    protected bool Hover;

    private SoundEffectInstance _soundEffectInstance;

    public new static Vector2 DefaultSize => DefaultMapping.ImageSize * 4;

    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(32, 16),
        Hitboxes = new[]
        {
            new Rectangle(2, 1, 28, 14),
            new Rectangle(1, 2, 30, 12)
        }
    };

    public EmptyButton() : this(Vector2.Zero, DefaultSize)
    {
    }

    public EmptyButton(Vector2 position) : this(position, DefaultSize)
    {
    }

    public EmptyButton(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public EmptyButton(Vector2 position, Vector2 size) : this(position, size, DefaultTexture, DefaultMapping)
    {
    }

    public EmptyButton(Vector2 position, Vector2 size, Texture2D texture, TextureHitboxMapping mapping) :
        base(position, size, texture, mapping)
    {
        _soundEffectInstance = Globals.SoundEffects.GetSfxInstance("ButtonSound");
    }

    ~EmptyButton()
    {
        _soundEffectInstance.Dispose();
    }

    public virtual void Update(GameTime gameTime, IHitbox toCheck)
    {
        bool isMouseHovering = false;
        foreach (Rectangle rectangle in toCheck.Hitbox)
            if (HitboxCheck(rectangle))
                isMouseHovering = true;

        if (isMouseHovering)
        {
            if (!Hover)
                InvokeEnterEventHandler();

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
                InvokeClickEventHandler();
        }
        else if (Hover)
            InvokeLeaveEventHandler();

        ImageLocation = new Rectangle(isMouseHovering ? (int) FrameSize.X : 0, 0, (int) FrameSize.X, (int) FrameSize.Y);
        Hover = isMouseHovering;
        base.Update(gameTime);
    }


    public Vector2 GetPosition()
        => Position;

    public virtual void Move(Vector2 newPosition)
        => Position = newPosition;

    protected void InvokeClickEventHandler()
    {
        _soundEffectInstance.Stop();
        Click?.Invoke(this);
        _soundEffectInstance.Play();
    }

    protected void InvokeEnterEventHandler()
        => Enter?.Invoke(this);

    protected void InvokeLeaveEventHandler()
        => Leave?.Invoke(this);

    public void ChangeSoundEffect(string key)
    {
        _soundEffectInstance.Dispose();
        _soundEffectInstance = Globals.SoundEffects.GetSfxInstance(key);
    }
}