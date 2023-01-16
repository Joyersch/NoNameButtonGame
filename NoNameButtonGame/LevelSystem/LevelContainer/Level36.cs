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
    class Level36 : SampleLevel
    {

        readonly TextButton[] BobIt;
        readonly Cursor cursor;
        readonly TextBuilder[] Marker;
        bool PlayingSequenz = false;
        string CurrentSequenz;
        string Sequenz;
        int CurrentSqAmm = 1;
        int PlayedSq;
        readonly int SqMax = 9;
        readonly float ShowTime = 250;
        AwesomeButton StartBtn;
        readonly Random rand;
        float SSGT;
        bool Display = false;
        public Level36(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 36 - Now you need to remember 9";
            BobIt = new TextButton[5];
            this.rand = rand;
            BobIt[0] = new TextButton(new Vector2(-320, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "0", "⬜", new Vector2(16, 16));
            BobIt[1] = new TextButton(new Vector2(-192, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "1", "⬜", new Vector2(16, 16));
            BobIt[2] = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "2", "⬜", new Vector2(16, 16));
            BobIt[3] = new TextButton(new Vector2(64, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "3", "⬜", new Vector2(16, 16));
            BobIt[4] = new TextButton(new Vector2(192, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "4", "⬜", new Vector2(16, 16));
            BobIt[0].Text.ChangeColor(new Color[1] { Color.Orange });
            BobIt[1].Text.ChangeColor(new Color[1] { Color.DarkRed });
            BobIt[2].Text.ChangeColor(new Color[1] { Color.Green });
            BobIt[3].Text.ChangeColor(new Color[1] { Color.Blue });
            BobIt[4].Text.ChangeColor(new Color[1] { Color.Purple });
            for (int i = 0; i < BobIt.Length; i++) {
                BobIt[i].Click += BtnEvent;
            }
            Sequenz = string.Empty;
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            Marker = new TextBuilder[2];
            Marker[0] = new TextBuilder("Simon says!", new Vector2(-70, -132), new Vector2(16, 16), null, 0);
            Marker[0].Position = Vector2.Zero - Marker[0].rectangle.Size.ToVector2() / 2;
            Marker[1] = new TextBuilder("", new Vector2(-40, 128), new Vector2(16, 16), null, 0);
            StartBtn = new AwesomeButton(new Vector2(-80, -32), new Vector2(160, 64), Globals.Content.GetHitboxMapping("startbutton"));
            StartBtn.Click += StartEvent;
        }

        private void StartEvent(object sender, EventArgs e) {
            PlayingSequenz = true;
            StartBtn = null;
        }

        private void BtnEvent(object sender, EventArgs e) {
            if (PlayingSequenz)
                return;
            CurrentSequenz += (sender as TextButton).Name;
            if (CurrentSequenz == Sequenz) {
                if (SqMax == CurrentSequenz.Length)
                    CallFinish(this, e);
                else {
                    CurrentSqAmm++;
                    CurrentSequenz = string.Empty;
                    PlayingSequenz = true;
                    PlayedSq = 0;
                    Marker[1] = new TextBuilder("", new Vector2(-40, 128), new Vector2(16, 16), null, 0);
                    Marker[1].Position = Vector2.Zero - Marker[1].rectangle.Size.ToVector2() / 2;
                }
                return;
            }
            if (Sequenz.Substring(0, CurrentSqAmm).StartsWith(CurrentSequenz)) {
                string mk = string.Empty;
                Color[] mkc = new Color[CurrentSqAmm];
                for (int i = 0; i < CurrentSqAmm; i++) {
                    mk += "⬜";

                }
                for (int i = 0; i < CurrentSequenz.Length; i++) {
                    switch (int.Parse(CurrentSequenz.Substring(i, 1).ToCharArray()[0].ToString())) {
                        case 0:
                            mkc[i] = Color.Orange;
                            break;
                        case 1:
                            mkc[i] = Color.DarkRed;
                            break;
                        case 2:
                            mkc[i] = Color.Green;
                            break;
                        case 3:
                            mkc[i] = Color.Blue;
                            break;
                        case 4:
                            mkc[i] = Color.Purple;
                            ;
                            break;
                    }
                }
                Marker[1] = new TextBuilder(mk, new Vector2(-40, 128), new Vector2(16, 16), mkc, 0);
                Marker[1].Position = Vector2.Zero - Marker[1].rectangle.Size.ToVector2() / 2;
                return;
            }
            CallFail(sender, e);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            if (!(StartBtn is null))
                StartBtn.Draw(spriteBatch);
            else {
                if (!PlayingSequenz) {
                    Marker[1].Draw(spriteBatch);
                }
                for (int i = 0; i < BobIt.Length; i++) {
                    BobIt[i].Draw(spriteBatch);
                }
            }

            Marker[0].Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);

            Marker[0].Update(gameTime);

            if (!(StartBtn is null))
                StartBtn.Update(gameTime, cursor.Hitbox[0]);
            else {

                if (!PlayingSequenz) {
                    Marker[1].Update(gameTime);
                } else {
                    SSGT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    while (SSGT > ShowTime) {
                        SSGT -= ShowTime;
                        BobIt[0].Text.ChangeColor(new Color[1] { Color.Orange });
                        BobIt[1].Text.ChangeColor(new Color[1] { Color.DarkRed });
                        BobIt[2].Text.ChangeColor(new Color[1] { Color.Green });
                        BobIt[3].Text.ChangeColor(new Color[1] { Color.Blue });
                        BobIt[4].Text.ChangeColor(new Color[1] { Color.Purple });
                        if (Display) {
                            if (PlayedSq >= CurrentSqAmm) {
                                PlayingSequenz = false;

                            } else {
                                int r;
                                if (PlayedSq + 1 <= Sequenz.Length && Sequenz.Length != 0) {
                                    r = int.Parse(Sequenz.ToCharArray()[PlayedSq].ToString());
                                } else {
                                    r = rand.Next(0, 5);
                                    Sequenz += r;
                                }
                                switch (r) {
                                    case 0:
                                        BobIt[0].Text.ChangeColor(new Color[1] { Color.MonoGameOrange });
                                        break;
                                    case 1:
                                        BobIt[1].Text.ChangeColor(new Color[1] { Color.Red });
                                        break;
                                    case 2:
                                        BobIt[2].Text.ChangeColor(new Color[1] { Color.LightGreen });
                                        break;
                                    case 3:
                                        BobIt[3].Text.ChangeColor(new Color[1] { Color.LightBlue });
                                        break;
                                    case 4:
                                        BobIt[4].Text.ChangeColor(new Color[1] { Color.LightPink });
                                        break;
                                }

                                PlayedSq++;
                            }
                        }
                        Display = !Display;
                    }
                }
                for (int i = 0; i < BobIt.Length; i++) {
                    BobIt[i].Update(gameTime, cursor.Hitbox[0]);
                }
            }
            cursor.Position = mousePosition - cursor.Size / 2;
        }
    }
}
