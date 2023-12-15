﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Listener;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Logic.Listener;
using MonoUtils.Ui.Objects;

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
    protected IHitbox Actuator;

    public static Effect effect;

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


    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        graphicsDevice.SetRenderTarget(Display.Target);
        graphicsDevice.Clear(new Color(50, 50, 50));

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
            transformMatrix: Camera.CameraMatrix, effect: effect);

        Draw(spriteBatch);

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
            if (obj is IManageable manageable &&
                manageable.Rectangle.Intersects(Camera.Rectangle.ExtendFromCenter(1.5F)))
                manageable.Draw(spriteBatch);
        }
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

    public virtual void Update(GameTime gameTime)
    {
        Camera.Update();

        PositionListener.Update(gameTime);
        RelativePositionListener.Update(gameTime);
        ColorListener.Update(gameTime);

        Mouse.Update(gameTime);

        if (InputReaderKeyboard.CheckKey(Keys.F7, true))
            Mouse.UseRelative = !Mouse.UseRelative;

        if (InputReaderKeyboard.CheckKey(Keys.F6, true))
            Mouse.SetMousePointerPositionToCenter();

        if (InputReaderKeyboard.CheckKey(Keys.Escape, true))
            Exit();

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