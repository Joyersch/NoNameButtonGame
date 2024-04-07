using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Helper;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Listener;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic.Listener;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;
using IUpdateable = MonoUtils.Logic.IUpdateable;

namespace NoNameButtonGame.LevelSystem;

public class SampleLevel : ILevel
{
    public event Action OnFail;
    public event Action OnExit;
    public event Action OnFinish;

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
    protected Cursor Cursor;

    private Text _cursorIndicator;

    protected SampleLevel(Display display, Vector2 window, Random random)
    {
        Display = display;
        Random = random;
        Window = window;
        Cursor = new Cursor
        {
            Layer = 0,
        };
        PositionListener = new PositionListener();
        RelativePositionListener = new RelativePositionListener();
        ColorListener = new ColorListener();

        AutoManaged = new List<object>();
        AutoManagedStatic = new List<object>();
        _cursorIndicator = new Text("[arrow]");
        _cursorIndicator.ChangeColor(Color.DeepSkyBlue);
        _cursorIndicator[0].Origin = new Vector2(2.5F, 2.5F);

        Camera = new Camera(Vector2.Zero, Display.Size);
        Mouse = new MousePointer(window, Camera)
        {
            UseRelative = true
        };
        Mouse.SetMousePointerPositionToCenter();
        PositionListener.Add(Mouse, Cursor);
        RelativePositionListener.Add(Camera, Mouse);
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var obj in AutoManaged)
        {
            if (obj is IInteractable interactable)
                interactable.UpdateInteraction(gameTime, Cursor);

            if (obj is IUpdateable manageable)
                manageable.Update(gameTime);

            if (obj is Action action)
                action.Invoke();
        }

        Camera.Update();
        Mouse.Update(gameTime);

        MoveHelper.RotateTowards(_cursorIndicator[0], Cursor);
        _cursorIndicator[0].Rotation += (float)(Math.PI / 4F);

        var position = Cursor.GetPosition() + Cursor.GetSize() * 0.5F;
        ;
        var sizeOffset = _cursorIndicator.GetSize() * 0.5F;
        var rectangleWidth = Camera.Rectangle.Width * 0.04F;
        var rectangleHeight = Camera.Rectangle.Height * 0.075F;

        var newPosition = position;
        if (position.X <= Camera.Rectangle.Left + rectangleWidth)
            newPosition.X = Camera.Rectangle.Left + rectangleWidth;

        if (position.X >= Camera.Rectangle.Right - rectangleWidth)
            newPosition.X = Camera.Rectangle.Right - rectangleWidth;

        if (position.Y <= Camera.Rectangle.Top + rectangleHeight)
            newPosition.Y = Camera.Rectangle.Top + rectangleHeight;

        if (position.Y >= Camera.Rectangle.Bottom - rectangleHeight)
            newPosition.Y = Camera.Rectangle.Bottom - rectangleHeight;

        _cursorIndicator.Move(newPosition);

        _cursorIndicator.Update(gameTime);
        PositionListener.Update(gameTime);
        RelativePositionListener.Update(gameTime);
        ColorListener.Update(gameTime);

        if (InputReaderKeyboard.CheckKey(Keys.F7, true))
            Mouse.UseRelative = !Mouse.UseRelative;

        if (InputReaderKeyboard.CheckKey(Keys.F6, true))
            Mouse.SetMousePointerPositionToCenter();

        if (InputReaderKeyboard.CheckKey(Keys.Escape, true))
            Exit();

        Cursor.Update(gameTime);
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        graphicsDevice.SetRenderTarget(Display.Target);

        graphicsDevice.Clear(new Color(50, 50, 50));

        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp,
            transformMatrix: Camera.CameraMatrix);

        Draw(spriteBatch);

        if (!Camera.Rectangle.Intersects(Cursor.Rectangle))
        {
            _cursorIndicator.Draw(spriteBatch);
        }

        spriteBatch.End();

        graphicsDevice.SetRenderTarget(null);

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

        graphicsDevice.Clear(Color.HotPink);

        spriteBatch.Draw(Display.Target, Display.Window, null, Color.White);

        DrawStatic(spriteBatch);

        spriteBatch.End();
    }

    protected virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var obj in AutoManaged)
        {
            if (obj is MonoUtils.Logic.IDrawable drawable &&
                drawable.Rectangle.Intersects(Camera.Rectangle.ExtendFromCenter(1.5F)))
                drawable.Draw(spriteBatch);
        }

        Cursor.Draw(spriteBatch);
        Mouse.DrawIndicator(spriteBatch);
    }

    protected virtual void DrawStatic(SpriteBatch spriteBatch)
    {
        foreach (var obj in AutoManagedStatic)
        {
            if (obj is IManageable manageable)
                manageable.Draw(spriteBatch);
        }

        Mouse.Draw(spriteBatch);
    }

    public virtual void SetScreen(Vector2 screen)
    {
        Window = screen;
        Mouse.UpdateWindow(screen);
    }

    protected virtual void Fail(object sender)
        => Fail();

    protected virtual void Fail()
        => OnFail?.Invoke();

    protected virtual void Finish(object sender)
        => Finish();

    protected virtual void Finish()
        => OnFinish?.Invoke();

    public virtual void Exit(object sender)
        => Exit();

    public virtual void Exit()
        => OnExit?.Invoke();
}