using System;
using System.Collections.Generic;
using Joyersch.Monogame;
using Joyersch.Monogame.Listener;
using Joyersch.Monogame.Logging;
using Joyersch.Monogame.Sound;
using Joyersch.Monogame.Storage;
using Joyersch.Monogame.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Helpers;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Listener;
using NoNameButtonGame.Ui;
using NoNameButtonGame.Ui.Text;
using IDrawable = NoNameButtonGame.IDrawable;
using IUpdateable = Joyersch.Monogame.IUpdateable;

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

    private MouseSettings _mouseSettings;
    private ScaleProvider _scaleProvider;

    protected readonly Display Display;
    public string Name;
    protected Random Random;

    protected readonly PositionListener PositionListener;
    protected readonly RelativePositionListener RelativePositionListener;
    protected readonly ColorListener ColorListener;
    protected readonly CalculatorCollection CalculatorCollection;

    protected readonly List<object> AutoManaged;
    protected readonly List<object> AutoManagedStaticFront;
    protected readonly List<object> AutoManagedStaticBack;
    protected readonly List<IScaleable> AutoScale;
    protected Cursor Cursor;

    private bool _canExit;

    private BasicText _cursorIndicator;

    protected SampleLevel(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> settingsAndSaveManager)
    {
        Display = scene.Display;
        Random = random;
        EffectsRegistry = effectsRegistry;

        PositionListener = new PositionListener();
        RelativePositionListener = new RelativePositionListener();
        ColorListener = new ColorListener();
        CalculatorCollection = new CalculatorCollection();
        _scaleProvider = new ScaleProvider(scene);
        
        _mouseSettings = settingsAndSaveManager.GetSetting<MouseSettings>();

        AutoManaged = [];
        AutoManagedStaticFront = [];
        AutoManagedStaticBack = [];
        AutoScale = [];

        Cursor = new Cursor(2F)
        {
            Layer = 0
        };
        AutoScale.Add(Cursor);

        _cursorIndicator = new BasicText("[arrow]", Vector2.Zero, 3f);
        _cursorIndicator.ChangeColor(Color.DeepSkyBlue);
        AutoScale.Add(_cursorIndicator);

        Camera = scene.Camera;
        // Set Camera to 0,0 as it is kept between levels
        Camera.Move(Vector2.Zero);

        Mouse = new MousePointer(scene)
        {
            UseRelative = true
        };
        Mouse.SetMousePointerPositionToCenter();
        PositionListener.Add(Mouse, Cursor);
        RelativePositionListener.Add(Camera, Mouse);
        scene.Display.OnResize += delegate
        {
            Camera.Calculate();
            CalculatorCollection.Apply();
            Log.Warning("resizing");
            
            foreach (var scaleable in AutoScale)
                scaleable.SetScale(_scaleProvider);
        };
        SetScaleAndCalculatePositions();
    }

    public void SetScaleAndCalculatePositions()
    {
        foreach (var scaleable in AutoScale)
            scaleable.SetScale(_scaleProvider);
        CalculatorCollection.Apply();
    }

    public virtual void Update(GameTime gameTime)
    {
        _cursorIndicator[0].Origin = new Vector2(8f, 0f);
        Mouse.Speed = _mouseSettings.Sensitivity;
        Mouse.Update(gameTime);
        RelativePositionListener.Update(gameTime);
        PositionListener.Update(gameTime);
        Cursor.Update(gameTime);

        var cameraPosition = Camera.Position;
        foreach (var obj in AutoManaged)
        {
            if (obj is IInteractable interactable)
                interactable.UpdateInteraction(gameTime, Cursor);

            if (obj is IUpdateable manageable)
                manageable.Update(gameTime);

            if (obj is Action action)
                action.Invoke();
        }

        if (cameraPosition != Camera.Position)
        {
            Camera.Calculate();
            Mouse.Update(gameTime);
            RelativePositionListener.Update(gameTime);
            PositionListener.Update(gameTime);
            Cursor.Update(gameTime);
        }

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
        ColorListener.Update(gameTime);

        if (!_canExit)
            _canExit = Keyboard.GetState()[Keys.Escape] == KeyState.Up;
        else if (Keyboard.GetState()[Keys.Escape] == KeyState.Down)
        {
            _canExit = false;
            Exit();
        }
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        graphicsDevice.SetRenderTarget(null);
        graphicsDevice.Clear(new Color(50, 50, 50));

        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp,
            transformMatrix: Camera.CameraMatrix);

        DrawStaticBack(spriteBatch);

        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp,
            transformMatrix: Camera.CameraMatrix);

        Draw(spriteBatch);

        if (!Camera.Rectangle.Intersects(Cursor.Rectangle))
            _cursorIndicator.Draw(spriteBatch);

        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

        DrawStaticFront(spriteBatch);

        spriteBatch.End();
    }

    protected virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var obj in AutoManaged)
        {
            if (obj is IDrawable drawable &&
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
        var effect = EffectsRegistry.GetInstance(Statics.Sfx.Wall);
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