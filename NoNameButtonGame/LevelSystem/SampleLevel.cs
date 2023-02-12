using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Camera;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.LevelSystem;

internal class SampleLevel : ILevel
{
    public event Action FailEventHandler;
    public event Action ExitEventHandler;
    public event Action FinishEventHandler;

    public event Action<string> CurrentMusicEventHandler;

    public CameraClass Camera;
    public Vector2 Window;
    public Vector2 cameraPosition;
    public Vector2 mousePosition;
    public Rectangle cameraRectangle;
    public int defaultWidth;
    public int defaultHeight;
    public string Name;

    private bool animationIn = true;
    private bool animationOut = false;
    private bool levelStarted = false;

    public SampleLevel(int defaultWidth, int defaultHeight, Vector2 window, Random random)
    {
        this.defaultWidth = defaultWidth;
        this.defaultHeight = defaultHeight;
        Window = window;
        Camera = new CameraClass(new Vector2(defaultWidth, defaultHeight));
        cameraPosition = Vector2.Zero;
        Mouse.SetPosition((int) Window.X / 2, (int) Window.Y / 2);
    }

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

    public virtual void SetScreen(Vector2 Screen) => Window = Screen;

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
    
    public virtual void CurrentMusic(string music)
        => CurrentMusicEventHandler?.Invoke(music);
}