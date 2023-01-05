using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using NoNameButtonGame.BeforeMaths;
using NoNameButtonGame.LevelSystem;
using System.IO;

namespace NoNameButtonGame
{
    public class NoNameGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private RenderTarget2D target2D;
        readonly float defaultWidth = 1280F;
        readonly float defaultHeight = 720F;
        LevelManager levelManager;
        Texture2D Mousepoint;
        Vector2 MousepointTopLeft;
        bool ShowActualMousePos = false;

        public NoNameGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Globals.Storage = new Storage(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                          "/NoNameButtonGame/data.txt");
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        private void Changesettings(Vector2 Res, bool step, bool full)
        {
            IsFixedTimeStep = step;
            //Apply settings
            if ((!_graphics.IsFullScreen && full) || (!full && _graphics.IsFullScreen))
                _graphics.ToggleFullScreen();
            _graphics.PreferredBackBufferWidth = (int) Res.X;
            _graphics.PreferredBackBufferHeight = (int) Res.Y;
            _graphics.ApplyChanges();

            //Write settings
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                      "/NoNameButtonGame/data.txt"))
            {
                sw.WriteLine(step);
                sw.WriteLine(full);
                sw.WriteLine(Res.X);
                sw.WriteLine(Res.Y);
                sw.WriteLine(Globals.Storage.GameData.MaxLevel);
            }

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

            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                             "/NoNameButtonGame/data.txt"))
            {
                using (StreamWriter sw =
                       new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                        "/NoNameButtonGame/data.txt"))
                {
                    sw.WriteLine(IsFixedTimeStep);
                    sw.WriteLine(_graphics.IsFullScreen);
                    sw.WriteLine("1280");
                    sw.WriteLine("720");
                }

                if (_graphics.IsFullScreen)
                    _graphics.ToggleFullScreen();
                _graphics.PreferredBackBufferWidth = 1280;
                _graphics.PreferredBackBufferHeight = 720;
                _graphics.ApplyChanges();
                IsFixedTimeStep = false;
            }
            else
            {
                try
                {
                    using (StreamReader sr = new StreamReader(
                               Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                               "/NoNameButtonGame/data.txt"))
                    {
                        IsFixedTimeStep = bool.Parse(sr.ReadLine());
                        bool full = bool.Parse(sr.ReadLine());
                        Globals.Storage.Settings.IsFullscreen = full;
                        if ((!_graphics.IsFullScreen && full) || (!full && _graphics.IsFullScreen))
                            _graphics.ToggleFullScreen();
                        int X = int.Parse(sr.ReadLine());
                        int Y = int.Parse(sr.ReadLine());
                        _graphics.PreferredBackBufferWidth = X;
                        _graphics.PreferredBackBufferHeight = Y;
                        _graphics.ApplyChanges();
                        Globals.Storage.GameData.MaxLevel = int.Parse(sr.ReadLine());
                    }
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
            }

            Globals.Content = Content;
            Globals.Storage.Settings.IsFixedStep = IsFixedTimeStep;

            #endregion


            target2D = new RenderTarget2D(GraphicsDevice, (int) defaultWidth, (int) defaultHeight);
            levelManager = new LevelManager((int) defaultHeight, (int) defaultWidth,
                new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Changesettings)
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

        //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
        Rectangle BackbufferBounds;
        float backbufferAspectRatio;
        float ScreenAspectRatio;
        float rx, ry, rw, rh;


        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            MousepointTopLeft = mouse.Position.ToVector2() - new Vector2(3, 3);
            base.Update(gameTime);


            levelManager.Update(gameTime);

            //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
            BackbufferBounds = GraphicsDevice.PresentationParameters.Bounds;
            backbufferAspectRatio = (float) BackbufferBounds.Width / BackbufferBounds.Height;
            ScreenAspectRatio = (float) target2D.Width / target2D.Height;

            rx = 0f;
            ry = 0f;
            rw = BackbufferBounds.Width;
            rh = BackbufferBounds.Height;
            if (backbufferAspectRatio > ScreenAspectRatio)
            {
                rw = rh * ScreenAspectRatio;
                rx = (BackbufferBounds.Width - rw) / 2f;
            }
            else if (backbufferAspectRatio < ScreenAspectRatio)
            {
                rh = rw / ScreenAspectRatio;
                ry = (BackbufferBounds.Height - rh) / 2f;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(target2D);
            GraphicsDevice.Clear(new Color(50, 50, 50));

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                null, transformMatrix: levelManager.GetCurrentCamera().CamMatrix);

            levelManager.Draw(_spriteBatch);

            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);


            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                null, null);

            Rectangle DesRec = new Rectangle((int) rx, (int) ry, (int) rw, (int) rh);
            GraphicsDevice.Clear(Color.HotPink);
            _spriteBatch.Draw(target2D, DesRec, null, Color.White);

            if (ShowActualMousePos)
                _spriteBatch.Draw(Mousepoint, new Rectangle(MousepointTopLeft.ToPoint(), new Point(6, 6)), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}