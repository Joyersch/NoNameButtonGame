using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.Buttons;

public class EmptyButton : GameObject, IMouseActions, IMoveable
{
    public event Action<object> LeaveEventHandler;
    public event Action<object> EnterEventHandler;
    public event Action<object> ClickEventHandler;
    protected bool Hover;
    protected readonly SoundEffect ClickEffect;
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
        ClickEffect = Globals.SoundEffects.GetEffect("ButtonSound");
    }

    public virtual void Update(GameTime gameTime, Rectangle mousePosition)
    {
        bool isMouseHovering = HitboxCheck(mousePosition);
        if (isMouseHovering)
        {
            if (!this.Hover)
                InvokeEnterEventHandler();

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
                InvokeClickEventHandler();
        }
        else if (this.Hover)
            InvokeLeaveEventHandler();

        ImageLocation = new Rectangle(isMouseHovering ? (int) FrameSize.X : 0, 0, (int) FrameSize.X, (int) FrameSize.Y);
        this.Hover = isMouseHovering;
        base.Update(gameTime);
    }


    public Vector2 GetPosition()
        => Position;

    public virtual bool Move(Vector2 newPosition)
    {
        Position = newPosition;
        return true;
    }

    protected void InvokeClickEventHandler()
    {
        if (_soundEffectInstance is not null)
        {
            _soundEffectInstance.Stop();
            _soundEffectInstance.Dispose();
            _soundEffectInstance = null;
        }

        _soundEffectInstance = ClickEffect.CreateInstance();
        Globals.SoundSettingsListener.AddSettingsLink(_soundEffectInstance);
        _soundEffectInstance.Play();
        ClickEventHandler?.Invoke(this);
    }

    protected void InvokeEnterEventHandler()
        => EnterEventHandler?.Invoke(this);

    protected void InvokeLeaveEventHandler()
        => LeaveEventHandler?.Invoke(this);
}