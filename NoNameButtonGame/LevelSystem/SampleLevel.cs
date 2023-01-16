using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Camera;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.LevelSystem
{
    class SampleLevel : MonoObject, ILevel
    {
        public event EventHandler FailEventHandler;
        public event EventHandler ExitEventHandler;
        public event EventHandler FinishEventHandler;

        public CameraClass Camera;
        public Vector2 Window;
        public Vector2 cameraPosition;
        public Vector2 mousePosition;
        public Rectangle cameraRectangle;
        public int defaultWidth;
        public int defaultHeight;
        public string Name;

        private LevelState levelEndState;

        private int OutMath;
        private readonly int OutMathValue = 90;
        private bool animationIn = true;
        private bool animationOut = false;
        private bool levelStarted = false;

        private float OutGT;

        enum LevelState
        {
            Fail,
            Win,
            Exit,
        }

        public SampleLevel(int defaultWidth, int defaultHeight, Vector2 window, Random random)
        {
            this.defaultWidth = defaultWidth;
            this.defaultHeight = defaultHeight;
            cameraPosition = new Vector2(0, -2291.5984F); //Dont ask why it just needs to be
            OutMath = OutMathValue;
            Window = window;
            Camera = new CameraClass(new Vector2(defaultWidth, defaultHeight));
            cameraPosition = Vector2.Zero;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            /*
            if ((animationIn || animationOut) && false)
            {
                OutGT += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

                while (OutGT > 8)
                {
                    OutGT -= 8;
                    Vector2 SinWaveRoute = new Vector2(0,
                        40F * (float) Math.Sin((float) OutMath / OutMathValue * (Math.PI * 1)));
                    OutMath--;
                    if (!animationOut)
                        cameraPosition += SinWaveRoute;
                    else
                        cameraPosition -= SinWaveRoute;

                    if (OutMath == 0)
                    {
                        animationIn = false;
                        OutMath = OutMathValue;
                        if (animationOut)
                        {
                            animationOut = false;
                            switch (levelEndState)
                            {
                                case LevelState.Fail:
                                    FailEventHandler(senderForEvent ?? this, argsForEvent ?? new EventArgs());
                                    break;
                                case LevelState.Win:
                                    FinishEventHandler(senderForEvent ?? this, argsForEvent ?? new EventArgs());
                                    break;
                                case LevelState.Exit:
                                    ExitEventHandler(senderForEvent ?? this, argsForEvent ?? new EventArgs());
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            cameraPosition = Vector2.Zero;
                        }

                        
                    }
                }
            }
*/
            if (!levelStarted)
            {
                Mouse.SetPosition((int) Window.X / 2, (int) Window.Y / 2);
                levelStarted = true;
            }

            Camera.Update(cameraPosition, new Vector2(0, 0));

            cameraRectangle = new Rectangle((cameraPosition - new Vector2(defaultWidth, defaultHeight)).ToPoint(),
                new Point(defaultWidth * 2, defaultHeight * 2));

            var mouseVector = Mouse.GetState().Position.ToVector2();
            var screenScale = new Vector2(Window.X / defaultWidth, Window.Y / defaultHeight);
            var offset = new Vector2(defaultWidth, defaultHeight) / Camera.Zoom / 2;
            mousePosition = mouseVector / screenScale / Camera.Zoom + cameraPosition - offset;
        }

        public virtual void SetScreen(Vector2 Screen) => Window = Screen;

        public virtual void CallFail(object s, EventArgs e)
            => FailEventHandler?.Invoke(s, e);

        public virtual void CallFail()
            => FailEventHandler?.Invoke(this, EventArgs.Empty);

        public virtual void CallFinish(object s, EventArgs e)
            => FinishEventHandler?.Invoke(s, e);

        public virtual void CallFinish()
            => FinishEventHandler?.Invoke(this, EventArgs.Empty);

        public virtual void CallExit(object s, EventArgs e)
            => ExitEventHandler?.Invoke(s, e);

        public virtual void CallExit()
            => ExitEventHandler?.Invoke(this, EventArgs.Empty);
    }
}