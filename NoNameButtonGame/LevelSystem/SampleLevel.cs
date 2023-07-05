using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Listener;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Logic.Listener;
using MonoUtils.Ui.Objects;

namespace NoNameButtonGame.LevelSystem;

public class SampleLevel : ILevel, IDisposable
{
    public event Action FailEventHandler;
    public event Action ExitEventHandler;
    public event Action FinishEventHandler;

    public event Action<string> CurrentMusicEventHandler;

    public readonly Camera Camera;
    public Vector2 Window { get; protected set; }
    protected readonly MousePointer Mouse;
    protected Vector2 CameraPosition => Camera.Position;

    protected readonly Display Display;
    public string Name;
    protected Random Random;

    protected readonly PositionListener PositionListener;
    protected readonly RelativePositionListener RelativePositionListener;
    protected readonly ColorListener ColorListener;

    protected readonly List<object> AutoManaged;
    protected readonly List<object> AutoManagedStatic;
    protected IHitbox Actuator;

    protected SampleLevel(Display display, Vector2 window, Random random)
    {
        Display = display;
        Random = random;
        Window = window;

        PositionListener = new PositionListener();
        RelativePositionListener = new RelativePositionListener();
        ColorListener = new ColorListener();

        AutoManaged = new List<object>();
        AutoManagedStatic = new List<object>();
        
        Camera = new Camera(Vector2.Zero, Display.Size);
        Mouse = new MousePointer(window, Camera, true)
        {
            UseRelative = true
        };
        Mouse.SetMousePointerPositionToCenter();
        RelativePositionListener.Add(Camera, Mouse);
    }
    

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var obj in AutoManaged)
        {
            if (obj is IManageable manageable &&
                manageable.Rectangle.Intersects(Camera.Rectangle.ExtendFromCenter(1.5F)))
                manageable.Draw(spriteBatch);
        }
        //Mouse.Draw(spriteBatch);
    }

    public virtual void DrawStatic(SpriteBatch spriteBatch)
    {
        foreach (var obj in AutoManagedStatic)
        {
            if (obj is IManageable manageable)
                manageable.Draw(spriteBatch);
        }
        Mouse.Draw(spriteBatch);
    }

    public virtual void Update(GameTime gameTime)
    {
        Camera.Update();

        PositionListener.Update(gameTime);
        RelativePositionListener.Update(gameTime);
        
        Mouse.Update(gameTime);
        
        if (InputReaderKeyboard.CheckKey(Keys.F7, true))
            Mouse.UseRelative = !Mouse.UseRelative;
        
        if (InputReaderKeyboard.CheckKey(Keys.F6, true))
            Mouse.SetMousePointerPositionToCenter();

        foreach (var obj in AutoManaged)
        {
            if (obj is IInteractable interactable)
                interactable.UpdateInteraction(gameTime, Actuator);

            if (obj is IManageable manageable)
                manageable.Update(gameTime);
        }
    }

    public virtual void SetScreen(Vector2 screen)
    {
        Window = screen;
        Mouse.UpdateWindow(screen);
    }

    protected virtual void Fail(object sender)
        => Fail();

    protected virtual void Fail()
        => FailEventHandler?.Invoke();

    protected virtual void Finish(object sender)
        => Finish();

    protected virtual void Finish()
        => FinishEventHandler?.Invoke();

    public virtual void Exit(object sender)
        => Exit();

    public virtual void Exit()
        => ExitEventHandler?.Invoke();

    protected virtual void CurrentMusic(string music)
        => CurrentMusicEventHandler?.Invoke(music);

    public void Dispose()
    {
        foreach (var g in AutoManaged)
        {
            if (g is IDisposable d)
                d.Dispose();
        }
    }
}