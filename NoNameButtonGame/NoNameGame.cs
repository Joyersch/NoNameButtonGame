using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using NoNameButtonGame.BeforeMaths;
using NoNameButtonGame.LevelSystem;
using Display = NoNameButtonGame.Display;

namespace NoNameButtonGame
{
    public class NoNameGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Display.Display display;
        LevelManager levelManager;
        Texture2D Mousepoint;
        Vector2 MousepointTopLeft;
        bool ShowActualMousePos = false;

        public NoNameGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Globals.Storage = new Storage(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                          "/NoNameButtonGame/");
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        private void ChangeSettings(Vector2 Res, bool step, bool full)
        {
            IsFixedTimeStep = step;
            //Apply settings
            if ((!_graphics.IsFullScreen && full) || (!full && _graphics.IsFullScreen))
                _graphics.ToggleFullScreen();
            _graphics.PreferredBackBufferWidth = (int) Res.X;
            _graphics.PreferredBackBufferHeight = (int) Res.Y;
            _graphics.ApplyChanges();

            Globals.Storage.Settings.Resolution.Width = (int) Res.X;
            Globals.Storage.Settings.Resolution.Height = (int) Res.Y;
            Globals.Storage.Settings.IsFullscreen = full;
            Globals.Storage.Settings.IsFixedStep = step;
            //Write storage
            Globals.Storage.Save();

            //Apply window changes
            levelManager.ChangeScreen(new Vector2(_graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight));
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
                Globals.Storage.Load();
                _graphics.PreferredBackBufferWidth = Globals.Storage.Settings.Resolution.Width;
                _graphics.PreferredBackBufferHeight = Globals.Storage.Settings.Resolution.Height;
                _graphics.ApplyChanges();
            }
            catch
            {
                if (_graphics.IsFullScreen)
                    _graphics.ToggleFullScreen();
                _graphics.PreferredBackBufferWidth = 1280;
                _graphics.PreferredBackBufferHeight = 720;
                _graphics.ApplyChanges();
                IsFixedTimeStep = false;
            }

            Globals.Content = Content;
            Globals.Storage.Settings.IsFixedStep = IsFixedTimeStep;

            #endregion

            display = new(GraphicsDevice);
            levelManager = new LevelManager((int) display.DefaultHeight, (int) display.DefaultWidth,
                new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), ChangeSettings)
            {
                ChangeWindowName = ChangeTitle
            };

            Mousepoint = Content.GetTHBox("mousepoint").Texture;

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

            display.Update(gameTime);

            levelManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(display.Target);
            GraphicsDevice.Clear(new Color(50, 50, 50));

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                null, transformMatrix: levelManager.GetCurrentCamera().CamMatrix);

            levelManager.Draw(_spriteBatch);

            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                null, null);
            
            GraphicsDevice.Clear(Color.HotPink);
            _spriteBatch.Draw(display.Target, display.Window, null, Color.White);

            if (ShowActualMousePos)
                _spriteBatch.Draw(Mousepoint, new Rectangle(MousepointTopLeft.ToPoint(), new Point(6, 6)), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LoadArgs(string args)
        {
            
        }
    }
}