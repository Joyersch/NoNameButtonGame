using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Listener;
using MonoUtils.Logic.Management;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.LevelSystem.Settings;
using IUpdateable = MonoUtils.Logic.IUpdateable;

namespace NoNameButtonGame.LevelSystem;

public class SampleLevel : ILevel
{
    public event Action OnFail;
    public event Action OnExit;
    public event Action OnFinish;

    public readonly Camera Camera;
    public Vector2 Window { get; protected set; }
    protected EffectsRegistry EffectsRegistry { get; }
    protected readonly MousePointer Mouse;
    protected Vector2 CameraPosition => Camera.Position;

    private MouseSettings _mouseSettings;

    protected readonly Display Display;
    public string Name;
    protected Random Random;

    protected readonly PositionListener PositionListener;
    protected readonly RelativePositionListener RelativePositionListener;
    protected readonly ColorListener ColorListener;

    protected readonly List<object> AutoManaged;
    protected readonly List<object> AutoManagedStaticFront;
    protected readonly List<object> AutoManagedStaticBack;
    protected Cursor Cursor;

    private bool _canExit;

    private Text _cursorIndicator;

    protected SampleLevel(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry, SettingsAndSaveManager settingsAndSaveManager)
    {
        Display = display;
        Random = random;
        Window = window;
        EffectsRegistry = effectsRegistry;
        Cursor = new Cursor
        {
            Layer = 0,
        };
        PositionListener = new PositionListener();
        RelativePositionListener = new RelativePositionListener();
        ColorListener = new ColorListener();

        _mouseSettings = settingsAndSaveManager.GetSetting<MouseSettings>();

        AutoManaged = new List<object>();
        AutoManagedStaticFront = new List<object>();
        AutoManagedStaticBack = new List<object>();
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
        Mouse.Speed = _mouseSettings.Sensitivity;
        Mouse.Update(gameTime);

        MoveHelper.RotateTowards(_cursorIndicator[0], Cursor);
        _cursorIndicator[0].Rotation += (float)(Math.PI / 4F);

        var position = Cursor.GetPosition() + Cursor.GetSize() * 0.5F;

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

        if (!_canExit)
            _canExit = Keyboard.GetState()[Keys.Escape] == KeyState.Up;
        else if (Keyboard.GetState()[Keys.Escape] == KeyState.Down)
        {
            _canExit = false;
            Exit();
        }

        Cursor.Update(gameTime);
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        #region Game
        graphicsDevice.SetRenderTarget(Display.Target);
        graphicsDevice.Clear(Color.Transparent);

        spriteBatch.Begin(samplerState:SamplerState.PointClamp, transformMatrix: Camera.CameraMatrix);

        Draw(spriteBatch);

        if (!Camera.Rectangle.Intersects(Cursor.Rectangle))
        {
            _cursorIndicator.Draw(spriteBatch);
        }

        spriteBatch.End();

        #endregion // Game

        graphicsDevice.SetRenderTarget(null);
        graphicsDevice.Clear(new Color(50, 50, 50));

        spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

        DrawStaticBack(spriteBatch);

        spriteBatch.Draw(Display.Target, Display.Window, null, Color.White);

        DrawStaticFront(spriteBatch);

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

    protected virtual void DrawStaticFront(SpriteBatch spriteBatch)
    {
        foreach (var obj in AutoManagedStaticFront)
        {
            if (obj is IManageable manageable)
                manageable.Draw(spriteBatch);
        }

        Mouse.Draw(spriteBatch);
    }

    protected virtual void DrawStaticBack(SpriteBatch spriteBatch)
    {
        foreach (var obj in AutoManagedStaticBack)
        {
            if (obj is IManageable manageable)
                manageable.Draw(spriteBatch);
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
    {
        var effect = EffectsRegistry.GetInstance("wall");
        effect?.Play();
        OnFail?.Invoke();
    }

    protected virtual void Finish(object sender)
        => Finish();

    public virtual void Finish()
        => OnFinish?.Invoke();

    public virtual void Exit(object sender)
        => Exit();

    public virtual void Exit()
        => OnExit?.Invoke();
}