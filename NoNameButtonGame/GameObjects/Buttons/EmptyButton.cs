using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Input;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Cache;
using NoNameButtonGame.LogicObjects;

namespace NoNameButtonGame.GameObjects.Buttons;

public class EmptyButton : GameObject, IMouseActions, IMoveable
{
    public event Action<object> LeaveEventHandler;
    public event Action<object> EnterEventHandler;
    public event Action<object> ClickEventHandler;
    protected bool _hover;
    protected SoundEffect clickEffect;
    private SoundEffectInstance soundEffectInstance;

    public EmptyButton(Vector2 position) : this(position, DefaultSize)
    {
    }

    public EmptyButton(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public EmptyButton(Vector2 position, Vector2 canvas) : base(position, canvas)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<EmptyButton>();
        clickEffect = Globals.SoundEffects.GetEffect("ButtonSound");
    }

    public virtual void Update(GameTime gameTime, Rectangle mousePosition)
    {
        bool hover = HitboxCheck(mousePosition);
        if (hover)
        {
            if (!_hover)
                InvokeEnterEventHandler();

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
                InvokeClickEventHandler();
        }
        else if (_hover)
            InvokeLeaveEventHandler();

        ImageLocation = new Rectangle(hover ? (int) FrameSize.X : 0, 0, (int) FrameSize.X, (int) FrameSize.Y);
        _hover = hover;
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

    public static Vector2 DefaultSize => new Vector2(128, 64);
}