using System;
using Joyersch.Monogame.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Ui;

public sealed class ScaleDeviceHandler
{
    private readonly Scene _scene;
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly GraphicsAdapter _graphicsAdapter;
    private readonly GameWindow _window;

    private readonly ScaleProvider _scaleProvider;

    public event Action<ScaleProvider> ScaleChanged;

    public ScaleDeviceHandler(Scene scene, GraphicsDeviceManager graphicsDeviceManager, GraphicsDevice graphicsDevice,
        GraphicsAdapter graphicsAdapter, GameWindow window)
    {
        _scene = scene;
        _graphicsDeviceManager = graphicsDeviceManager;
        _graphicsDevice = graphicsDevice;
        _graphicsAdapter = graphicsAdapter;
        _window = window;
        _scaleProvider = new ScaleProvider(scene);
    }

    public ScaleDeviceHandler ScaleToScreen(float scale = 1F)
    {
        _graphicsDeviceManager.PreferredBackBufferWidth = (int)(_graphicsAdapter.CurrentDisplayMode.Width * scale);
        _graphicsDeviceManager.PreferredBackBufferHeight = (int)(_graphicsAdapter.CurrentDisplayMode.Height * scale);
        _graphicsDeviceManager.ApplyChanges();

        ScaleChanged?.Invoke(_scaleProvider);
        return this;
    }

    public ScaleDeviceHandler Fullscreen()
    {
        _graphicsDeviceManager.IsFullScreen = true;
        _window.IsBorderless = true;
        _graphicsDeviceManager.ApplyChanges();
        return this;
    }

    public ScaleDeviceHandler Windowed()
    {
        _window.IsBorderless = false;
        _graphicsDeviceManager.IsFullScreen = false;
        _graphicsDeviceManager.ApplyChanges();
        return this;
    }

    public void ApplyResolution(Resolution resolution)
    {
        _graphicsDeviceManager.PreferredBackBufferWidth = resolution.Width;
        _graphicsDeviceManager.PreferredBackBufferHeight = resolution.Height;
        _graphicsDeviceManager.ApplyChanges();

        _scene.Display.Update();
        _scene.Camera.Calculate();

        ScaleChanged?.Invoke(_scaleProvider);
    }
}