using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Input;

namespace NoNameButtonGame.GameObjects.Buttons;

public class EmptyButton : GameObject, IMouseActions, IMoveable
{
    public event Action<object> LeaveEventHandler;
    public event Action<object> EnterEventHandler;
    public event Action<object> ClickEventHandler;
    protected bool hover;
    protected SoundEffect clickEffect;
    private SoundEffectInstance soundEffectInstance;

    public EmptyButton(Vector2 position) : this(position, DefaultSize)
    {
    }

    public EmptyButton(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public EmptyButton(Vector2 position, Vector2 size) : base(position, size)
    {
    }

    public override void Initialize()
    {
        textureHitboxMapping = Globals.Textures.GetMappingFromCache<EmptyButton>();
        clickEffect = Globals.SoundEffects.GetEffect("ButtonSound");
    }

    public virtual void Update(GameTime gameTime, Rectangle mousePosition)
    {
        bool isMouseHovering = HitboxCheck(mousePosition);
        if (isMouseHovering)
        {
            if (!this.hover)
                InvokeEnterEventHandler();

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
                InvokeClickEventHandler();
        }
        else if (this.hover)
            InvokeLeaveEventHandler();

        ImageLocation = new Rectangle(isMouseHovering ? (int) FrameSize.X : 0, 0, (int) FrameSize.X, (int) FrameSize.Y);
        this.hover = isMouseHovering;
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
        if (soundEffectInstance is not null)
        {
            soundEffectInstance.Stop();
            soundEffectInstance.Dispose();
            soundEffectInstance = null;
        }
        soundEffectInstance = clickEffect.CreateInstance();
        Globals.SoundSettingsLinker.AddSettingsLink(soundEffectInstance);
        soundEffectInstance.Play();
        ClickEventHandler?.Invoke(this);
    }

    protected void InvokeEnterEventHandler()
        => EnterEventHandler?.Invoke(this);

    protected void InvokeLeaveEventHandler()
        => LeaveEventHandler?.Invoke(this);

    public new static Vector2 DefaultSize => new Vector2(128, 64);
}