using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
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
        
        LevelManager levelManager;
        Texture2D Mousepoint;
        Vector2 MousepointTopLeft;
        bool ShowActualMousePos = false;

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

            #region LoadArgs

            string[] args = Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-showmouse")
                {
                    ShowActualMousePos = true;
                }
            }

            #endregion

            IsFixedTimeStep = false;


            #region Storage

            if (!Directory.Exists(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/NoNameButtonGame"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                          "/NoNameButtonGame");
            }

            try
            {
                _storage.Load();
            }
            catch
            {
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

            Globals.Content = Content;

            #endregion

            _display = new(GraphicsDevice);
            levelManager = new LevelManager((int) _display.DefaultHeight, (int) _display.DefaultWidth,
                new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), _storage)
            {
                ChangeWindowName = ChangeTitle
            };

            Mousepoint = Content.GetHitboxMapping("mousepoint").Texture;

            //CamPos = new Vector2(button[0].Size.X / 2, button[0].Size.Y / 2);
            //CamPos = new Vector2(700, 400);
        }

        private void ChangeTitle(string NewName)
        {
            Window.Title = NewName;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            MousepointTopLeft = mouse.Position.ToVector2() - new Vector2(3, 3);
            base.Update(gameTime);

            _display.Update(gameTime);

            levelManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_display.Target);
            GraphicsDevice.Clear(new Color(50, 50, 50));

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                null, transformMatrix: levelManager.GetCurrentCamera().CamMatrix);

            levelManager.Draw(_spriteBatch);

            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                null, null);

            GraphicsDevice.Clear(Color.HotPink);
            _spriteBatch.Draw(_display.Target, _display.Window, null, Color.White);

            if (ShowActualMousePos)
                _spriteBatch.Draw(Mousepoint, new Rectangle(MousepointTopLeft.ToPoint(), new Point(6, 6)), Color.White);

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
            levelManager?.ChangeScreen(new Vector2(_graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight));
            _storage.Save();
        }
    }
}