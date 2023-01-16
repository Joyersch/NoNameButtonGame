using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.LevelSystem;
using Display = NoNameButtonGame.Display;

namespace NoNameButtonGame
{
    public class NoNameGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Display.Display _display;
        private Storage _storage;
        private LevelManager levelManager;

        private bool ShowActualMousePos = false;
        private MousePointer _mousePointer;

        public NoNameGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _storage = new Storage(Globals.SaveDirectory);

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Read argument(s)
            ShowActualMousePos = Environment.GetCommandLineArgs().Any(a => a == "-mp");

            // Check Save directory
            if (!Directory.Exists(Globals.SaveDirectory))
                Directory.CreateDirectory(Globals.SaveDirectory);

            // Load or generate settings file (as well as save)
            try
            {
                _storage.Load();
            }
            catch
            {
                // fallback default settings
                _storage.Settings.Resolution.Width = 1280;
                _storage.Settings.Resolution.Height = 720;
                _storage.Settings.IsFullscreen = false;
                _storage.Settings.IsFixedStep = true;
                _storage.Save();
            }
            finally
            {
                SettingsChanged(_storage.Settings, EventArgs.Empty);
                _storage.Settings.HasChanged += SettingsChanged;
            }

            // will be removed
            Globals.Content = Content;
            _display = new(GraphicsDevice);
            _mousePointer = new MousePointer();

            levelManager = new LevelManager(_display, _storage);
            levelManager.ChangeWindowName += ChangeTitle;
            levelManager.CloseGameEventHandler += Exit;
        }

        private void ChangeTitle(string NewName)
        {
            Window.Title = NewName;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MousePointer.TextureHitboxMapping = Content.GetHitboxMapping("mousepoint");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _mousePointer.Update(gameTime);

            _display.Update(gameTime);

            levelManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_display.Target);
            GraphicsDevice.Clear(new Color(50, 50, 50));

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
                transformMatrix: levelManager.CurrentCamera.CameraMatrix);

            levelManager.Draw(_spriteBatch);

            _spriteBatch.End();
            
            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            GraphicsDevice.Clear(Color.HotPink);
            
            _spriteBatch.Draw(_display.Target, _display.Window, null, Color.White);

            if (ShowActualMousePos)
                _mousePointer.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SettingsChanged(object obj, EventArgs e)
        {
            if (obj is not Settings settings)
                return;

            IsFixedTimeStep = settings.IsFixedStep;

            if ((!_graphics.IsFullScreen && settings.IsFullscreen) ||
                (!settings.IsFullscreen && _graphics.IsFullScreen))
                _graphics.ToggleFullScreen();
            
            _graphics.PreferredBackBufferWidth = settings.Resolution.Width;
            _graphics.PreferredBackBufferHeight = settings.Resolution.Height;
            _graphics.ApplyChanges();

            // Update level screen
            _storage.Save();
        }
    }
}