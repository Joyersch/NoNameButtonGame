using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem;

public class SampleLevel : ILevel, IDisposable
{
    public event Action FailEventHandler;
    public event Action ExitEventHandler;
    public event Action FinishEventHandler;

    public event Action<string> CurrentMusicEventHandler;

    public readonly Camera Camera;
    public Vector2 Window;
    protected readonly MousePointer Mouse;
    protected Vector2 CameraPosition => Camera.Position;

    protected readonly Display.Display Display;
    public string Name;
    protected Random Random;

    protected readonly PositionListener PositionListener;
    protected readonly ColorListener ColorListener;

    protected readonly List<object> AutoManaged;
    protected IHitbox Actuator;

    protected SampleLevel(Display.Display display, Vector2 window, Random random)
    {
        Display = display;
        Random = random;
        Window = window;

        PositionListener = new PositionListener();
        ColorListener = new ColorListener();

        AutoManaged = new List<object>();

        Camera = new Camera(Vector2.Zero, NoNameButtonGame.Display.Display.Size);
        Mouse = new MousePointer();
        SetMousePositionToCenter();
    }

    protected void SetMousePositionToCenter()
        => Microsoft.Xna.Framework.Input.Mouse.SetPosition((int) Window.X / 2, (int) Window.Y / 2);


    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var obj in AutoManaged)
        {
            if (obj is IManageable manageable &&
                manageable.Rectangle.Intersects(Camera.Rectangle.ExtendFromCenter(1.5F)))
                manageable.Draw(spriteBatch);
        }
    }

    public virtual void DrawStatic(SpriteBatch spriteBatch)
    {
        foreach (var obj in AutoManaged)
        {
            if (obj is IManageable manageable)
                manageable.DrawStatic(spriteBatch);
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        Camera.Update();

        var mouseVector = Microsoft.Xna.Framework.Input.Mouse.GetState().Position.ToVector2();
        var screenScale = new Vector2(Window.X / NoNameButtonGame.Display.Display.Width,
            Window.Y / NoNameButtonGame.Display.Display.Height);
        var offset = NoNameButtonGame.Display.Display.Size / Camera.Zoom / 2;
        Mouse.Move(mouseVector / screenScale / Camera.Zoom + Camera.Position - offset);
        Mouse.Update(gameTime);

        PositionListener.Update(gameTime);

        foreach (var obj in AutoManaged)
        {
            if (obj is IInteractable interactable)
                interactable.UpdateInteraction(gameTime, Actuator);

            if (obj is IManageable manageable)
                manageable.Update(gameTime);
        }
    }

    public virtual void SetScreen(Vector2 screen) => Window = screen;

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