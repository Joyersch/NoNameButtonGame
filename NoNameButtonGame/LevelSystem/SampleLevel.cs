using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Input;
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
    protected readonly MousePointer _mouse;
    protected Vector2 CameraPosition => Camera.Position;

    protected Display.Display _display;
    public string Name;
    protected Random Random;

    protected readonly PositionListener PositionListener;
    protected readonly ColorListener ColorListener;

    protected readonly List<object> AutoManaged;
    protected IHitbox Actuator;

    protected SampleLevel(Display.Display display, Vector2 window, Random random)
    {
        _display = display;
        Random = random;
        Window = window;

        PositionListener = new PositionListener();
        ColorListener = new ColorListener();

        AutoManaged = new List<object>();
        
        Camera = new Camera(Vector2.Zero, Display.Display.Size);
        _mouse = new MousePointer();
        SetMousePositionToCenter();
    }

    protected void SetMousePositionToCenter()
        => Mouse.SetPosition((int) Window.X / 2, (int) Window.Y / 2);


    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var obj in AutoManaged)
        {
            if (obj is GameObject gameObject)
                gameObject.Draw(spriteBatch);
            if (obj is Text text)
                text.Draw(spriteBatch);
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        Camera.Update();

        var mouseVector = Mouse.GetState().Position.ToVector2();
        var screenScale = new Vector2(Window.X / Display.Display.Width, Window.Y / Display.Display.Height);
        var offset = Display.Display.Size / Camera.Zoom / 2;
        _mouse.Move(mouseVector / screenScale / Camera.Zoom + Camera.Position - offset);
        _mouse.Update(gameTime);

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