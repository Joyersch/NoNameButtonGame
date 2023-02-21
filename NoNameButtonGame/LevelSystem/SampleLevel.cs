using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.LevelSystem;

internal class SampleLevel : ILevel
{
    public event Action FailEventHandler;
    public event Action ExitEventHandler;
    public event Action FinishEventHandler;

    public event Action<string> CurrentMusicEventHandler;

    public readonly CameraClass Camera;
    public Vector2 Window;
    protected Vector2 CameraPosition;
    protected Vector2 MousePosition;
    protected Rectangle CameraRectangle;
    protected readonly int DefaultWidth;
    protected readonly int DefaultHeight;
    public string Name;
    protected Random Random;

    protected SampleLevel(int defaultWidth, int defaultHeight, Vector2 window, Random random)
    {
        this.DefaultWidth = defaultWidth;
        this.DefaultHeight = defaultHeight;
        this.Random = random;
        Window = window;
        Camera = new CameraClass(new Vector2(defaultWidth, defaultHeight));
        CameraPosition = Vector2.Zero;
        SetMousePositionToCenter();
    }

    protected void SetMousePositionToCenter()
        => Mouse.SetPosition((int) Window.X / 2, (int) Window.Y / 2);


    public virtual void Draw(SpriteBatch spriteBatch)
    {
        throw new NotImplementedException();
    }

    public virtual void Update(GameTime gameTime)
    {
        Camera.Update(CameraPosition, new Vector2(0, 0));

        CameraRectangle = new Rectangle((CameraPosition - new Vector2(DefaultWidth, DefaultHeight)).ToPoint(),
            new Point(DefaultWidth * 2, DefaultHeight * 2));

        var mouseVector = Mouse.GetState().Position.ToVector2();
        var screenScale = new Vector2(Window.X / DefaultWidth, Window.Y / DefaultHeight);
        var offset = new Vector2(DefaultWidth, DefaultHeight) / Camera.Zoom / 2;
        MousePosition = mouseVector / screenScale / Camera.Zoom + CameraPosition - offset;
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
}