using System;
using System.Collections.Generic;
using System.Text;

using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;

using NoNameButtonGame.Interfaces;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level5 : SampleLevel
    {

        readonly HoldButton button;
        readonly Cursor cursor;
        readonly TextBuilder[] Infos;
        readonly LockButton lockbutton;
        readonly Random rand;
        float GT;
        public Level5(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 5 - MORE BUTTONS!";
            this.rand = rand;
            button = new HoldButton(new Vector2(-220, -100), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton")) {
                EndHoldTime = 6900
            };
            button.Click += EmptyBtnEvent;
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            Infos = new TextBuilder[2];
            Infos[0] = new TextBuilder("<-- Hold this button till the timer runs out!", new Vector2(-64, -72), new Vector2(8,8), null, 0);
            Infos[1] = new TextBuilder("<-- This one will be unlocked then", new Vector2(-64, 32), new Vector2(8, 8), null, 0);
            for (int i = 0; i < Infos.Length; i++) {
                Color[] c = new Color[Infos[i].Text.Length];
                for (int a = 0; a < Infos[i].Text.Length; a++) {
                    c[a] = new Color(rand.Next(64, 255), rand.Next(64, 255), rand.Next(64, 255));
                }
                Infos[i].ChangeColor(c);
            }
            lockbutton = new LockButton(new Vector2(-220, 0), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton"), true);
            lockbutton.Click += BtnEvent;
            
        }


        private void EmptyBtnEvent(object sender, EventArgs e) {
            lockbutton.Locked = !lockbutton.Locked;
        }

        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }
        private void WallEvent(object sender, EventArgs e) {
            CallReset(sender, e);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            button.Draw(spriteBatch);
            for (int i = 0; i < Infos.Length; i++) {
                Infos[i].Draw(spriteBatch);
            }
            lockbutton.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while(GT > 512) {
                GT -= 512;
                for (int i = 0; i < Infos.Length; i++) {
                    Color[] c = new Color[Infos[i].Text.Length];
                    for (int a = 0; a < Infos[i].Text.Length; a++) {
                        c[a] = new Color(rand.Next(63, 255), rand.Next(63, 255), rand.Next(63, 255));
                    }
                    Infos[i].ChangeColor(c);
                }
                
            }
            cursor.Update(gameTime);
            base.Update(gameTime);
            for (int i = 0; i < Infos.Length; i++) {
                Infos[i].Update(gameTime);
            }
            cursor.Position = mousePosition - cursor.Size / 2;
            button.Update(gameTime, cursor.Hitbox[0]);
            lockbutton.Update(gameTime, cursor.Hitbox[0]);
        }
    }
}
