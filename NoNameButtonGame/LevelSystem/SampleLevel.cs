using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Joyersch.Camera;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.GameObjects;
using Microsoft.Xna.Framework.Input;
namespace NoNameButtonGame.LevelSystem
{
    class SampleLevel : MonoObject, ILevel
    {
        public event EventHandler Fail;
        public event EventHandler Reset;
        public event EventHandler Finish;
        public CameraClass Camera;

        public Vector2 Window;
        public Vector2 cameraPosition;
        public Vector2 mousePosition;
        public Rectangle cameraRectangle;
        public int defaultWidth;
        public int defaultHeight;
        public string Name;
        Lstate levelEndState;

        int OutMath;
        readonly int OutMathValue = 90;
        bool animationIn = true;
        bool animationOut = false;
        bool levelStarted = false;
        
        float OutGT;
        object senderForEvent;
        EventArgs argsForEvent;
        enum Lstate
        {
            Fail,
            Win,
            Reset,
        }
        public SampleLevel(int defaultWidth, int defaultHeight, Vector2 window, Random random) {
            this.defaultWidth = defaultWidth;
            this.defaultHeight = defaultHeight;
            cameraPosition = new Vector2(0, -2291.5984F); //Dont ask why it just needs to be
            OutMath = OutMathValue;
            Window = window;
            Camera = new CameraClass(new Vector2(defaultWidth, defaultHeight));
            
        }

        public override void Draw(SpriteBatch spriteBatch) {
            throw new NotImplementedException();
        }
        
        public override void Update(GameTime gameTime) {
            if (animationIn || animationOut) {
                OutGT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                while (OutGT > 8) {
                    OutGT -= 8;
                    Vector2 SinWaveRoute = new Vector2(0, 40F * (float)Math.Sin((float)OutMath / OutMathValue * (Math.PI * 1 )));
                    OutMath--;
                    if (!animationOut)
                        cameraPosition += SinWaveRoute;
                    else
                        cameraPosition -= SinWaveRoute;

                    if (OutMath == 0) {
                        animationIn = false;
                        OutMath = OutMathValue;
                        if (animationOut) {
                            animationOut = false;
                            switch (levelEndState) {
                                case Lstate.Fail:
                                    Fail(senderForEvent ?? this, argsForEvent ?? new EventArgs());
                                    break;
                                case Lstate.Win:
                                    Finish(senderForEvent ?? this, argsForEvent ?? new EventArgs());
                                    break;
                                case Lstate.Reset:
                                    Reset(senderForEvent ?? this, argsForEvent ?? new EventArgs());
                                    break;
                                default:
                                    break;
                            }
                            
                        } else {
                            cameraPosition = Vector2.Zero;
                        }
                        if (!levelStarted) {
                            Mouse.SetPosition((int)Window.X / 2, (int)Window.Y / 2);
                            levelStarted = true;
                        }
                            
                    }
                }

            }

            {

                Camera.Update(cameraPosition, new Vector2(0, 0));
                cameraRectangle = new Rectangle((cameraPosition - new Vector2(defaultWidth, defaultHeight)).ToPoint(), new Point(defaultWidth * 2, defaultHeight * 2));
                if (!(animationIn || animationOut)) {
                    MouseState mouseState = Mouse.GetState();
                    Vector2 VecMouse = mouseState.Position.ToVector2();
                    float TargetScreenDifX = Window.X / defaultWidth;
                    float TargetScreenDifY = Window.Y / defaultHeight;
                    Vector2 VMP = new Vector2(VecMouse.X / TargetScreenDifX, VecMouse.Y / TargetScreenDifY);
                    mousePosition = new Vector2(VMP.X / Camera.Zoom + cameraPosition.X - (defaultWidth / Camera.Zoom) / 2, VMP.Y / Camera.Zoom + cameraPosition.Y - (defaultHeight / Camera.Zoom) / 2);

                }
            }
            //if (Raigy.Input.InputReaderKeyboard.CheckKey(Keys.Right, true)) {
            //    CamPos.X += 45;
            //}
            //if (Raigy.Input.InputReaderKeyboard.CheckKey(Keys.Left, true)) {
            //    CamPos.X -= 45;
            //}
            //if (Raigy.Input.InputReaderKeyboard.CheckKey(Keys.Up, true)) {
            //    CamPos.Y -= 45;
            //}
            //if (Raigy.Input.InputReaderKeyboard.CheckKey(Keys.Down, true)) {
            //    CamPos.Y += 45;
            //}
        }

        public virtual void SetScreen(Vector2 Screen) {
            Window = Screen;
        }
        public virtual void CallFinish() {
            if (Finish != null && Finish.GetInvocationList().Length > 0) {
                animationOut = true;
                levelEndState = Lstate.Win;
            }
                
        }
        public virtual void CallFail() {
            if (Fail != null && Fail.GetInvocationList().Length > 0) {
                animationOut = true;
                levelEndState = Lstate.Fail;
            }

        }
        public virtual void CallReset() {
            if (Reset != null && Reset.GetInvocationList().Length > 0) {
                animationOut = true;
                levelEndState = Lstate.Reset;
            }

        }
        public virtual void CallFinish(object s, EventArgs e) {
            if (Finish != null && Finish.GetInvocationList().Length > 0) {
                senderForEvent = s;
                argsForEvent = e;
                animationOut = true;
                levelEndState = Lstate.Win;
            }

        }
        public virtual void CallFail(object s, EventArgs e) {
            if (Fail != null && Fail.GetInvocationList().Length > 0) {
                senderForEvent = s;
                argsForEvent = e;
                animationOut = true;
                levelEndState = Lstate.Fail;
            }

        }
        public virtual void CallReset(object s, EventArgs e) {
            if (Reset != null && Reset.GetInvocationList().Length > 0) {
                senderForEvent = s;
                argsForEvent = e;
                animationOut = true;
                levelEndState = Lstate.Reset;
            }

        }
    }
}
