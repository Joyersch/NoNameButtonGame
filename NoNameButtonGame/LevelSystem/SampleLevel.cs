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
    protected Vector2 cameraPosition;
    protected Vector2 mousePosition;
    protected Rectangle cameraRectangle;
    protected readonly int defaultWidth;
    protected readonly int defaultHeight;
    public string Name;
    protected Random random;

    protected SampleLevel(int defaultWidth, int defaultHeight, Vector2 window, Random random)
    {
        this.defaultWidth = defaultWidth;
        this.defaultHeight = defaultHeight;
        this.random = random;
        Window = window;
        Camera = new CameraClass(new Vector2(defaultWidth, defaultHeight));
        cameraPosition = Vector2.Zero;
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
        Camera.Update(cameraPosition, new Vector2(0, 0));

        cameraRectangle = new Rectangle((cameraPosition - new Vector2(defaultWidth, defaultHeight)).ToPoint(),
            new Point(defaultWidth * 2, defaultHeight * 2));

        var mouseVector = Mouse.GetState().Position.ToVector2();
        var screenScale = new Vector2(Window.X / defaultWidth, Window.Y / defaultHeight);
        var offset = new Vector2(defaultWidth, defaultHeight) / Camera.Zoom / 2;
        mousePosition = mouseVector / screenScale / Camera.Zoom + cameraPosition - offset;
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