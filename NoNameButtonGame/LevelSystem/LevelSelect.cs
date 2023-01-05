using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Text;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.BeforeMaths;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class LevelSelect : SampleLevel
    {
        readonly TextButton[] levelButton;
        readonly TextButton[] Down;
        readonly TextButton[] Up;
        readonly Cursor mouseCursor;
        readonly int LevelAmmount = 1000;
        public LevelSelect(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level Selection";
            LevelAmmount = Globals.GameData.MaxLevel;
            levelButton = new TextButton[LevelAmmount];
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            int Screen = LevelAmmount / 30;
            Down = new TextButton[Screen];
            Up = new TextButton[Screen];
            for (int i = 0; i < Screen; i++) {
                
                Down[i] = new TextButton(new Vector2(-300, 138 + (defaultHeight / Camera.Zoom) * i), new Vector2(64, 32), Globals.Content.GetTHBox("minibutton"),"", "⬇", new Vector2(16, 16));
                Down[i].Click += MoveDown;
                
                Up[i] = new TextButton(new Vector2(-300, 190 + (defaultHeight / Camera.Zoom) * i), new Vector2(64, 32), Globals.Content.GetTHBox("minibutton"),"", "⬆", new Vector2(16, 16));
                Up[i].Click += MoveUp;
            }
            
            for (int i = 0; i < LevelAmmount; i++) {
                levelButton[i] = new TextButton(new Vector2(-200 + 100 * (i % 5), -140 + 50 * (i / 5) + 60 * (int)(i / 30)), new Vector2(64, 32), Globals.Content.GetTHBox("minibutton"), (i + 1).ToString(), (i + 1).ToString(), new Vector2(16, 16));
            levelButton[i].Click += SelectLevel;
            } 

        }
        bool bMove = false;
        bool bUp = false;
        int CTicks = 0;
        private void SelectLevel(object sender, EventArgs e) {
            CallFinish(sender,e);
        }
        private void MoveDown(object sender, EventArgs e) {
            bMove = true;
            bUp = false;
            CTicks = 40;
        }
        private void MoveUp(object sender, EventArgs e) {
            bMove = true;
            bUp = true;
            CTicks = 40;
        }
       
        public override void Draw(SpriteBatch sp) {
            
            for (int i = 0; i < LevelAmmount; i++) {
                if (levelButton[i].rec.Intersects(cameraRectangle))
                levelButton[i].Draw(sp);
            }
            for (int i = 0; i < Down.Length; i++) {
                if (Down[i].rec.Intersects(cameraRectangle))
                    Down[i].Draw(sp);
                if (Up[i].rec.Intersects(cameraRectangle))
                    Up[i].Draw(sp);
            }
            mouseCursor.Draw(sp);
        }
        float GT;
        public override void Update(GameTime gt) {
            base.Update(gt);
            if (bMove) {
                GT += (float)gt.ElapsedGameTime.TotalMilliseconds;
                while (GT > 10) {
                    GT -= 10;
                    Vector2 SinWaveRoute = new Vector2(0, 12.2F * (float)Math.Sin((float)CTicks / 50 * Math.PI));
                    if (bUp)
                        cameraPosition -= SinWaveRoute;
                    else
                        cameraPosition += SinWaveRoute;
                    CTicks--;
                    if (CTicks == 0) {
                        float ftmp = cameraPosition.Y % (defaultHeight / Camera.Zoom);
                        if (!bUp)
                            cameraPosition.Y += (defaultHeight / Camera.Zoom) - ftmp;
                        else
                            cameraPosition.Y -= ftmp;
                        bMove = false;
                    }

                }
            }
            mouseCursor.Update(gt);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            for (int i = 0; i < Down.Length; i++) {
                if (Down[i].rec.Intersects(cameraRectangle))
                    Down[i].Update(gt, mouseCursor.Hitbox[0]);
                if (Up[i].rec.Intersects(cameraRectangle))
                    Up[i].Update(gt, mouseCursor.Hitbox[0]);
            }

            for (int i = 0; i < LevelAmmount; i++) {
                if (levelButton[i].rec.Intersects(cameraRectangle))
                    levelButton[i].Update(gt, mouseCursor.Hitbox[0]);
            }
        }
    }
}
