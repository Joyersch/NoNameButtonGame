using System;
using System.Collections.Generic;
using System.Text;

using Joyersch.Obj;
using Joyersch.Input;
using Joyersch.Camera;

using NoNameButtonGame.Interfaces;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level9 : SampleLevel
    {

        readonly TextButton[] bobIt;
        readonly Cursor mouseCursor;
        readonly TextBuilder[] Marker;
        readonly int SqMax = 5;
        readonly Random rand;
        AwesomeButton StartBtn;
        bool PlayingSequenz = false;
        string CurrentSequenz;
        string Sequenz;
        int CurrentSqAmm = 1;
        int PlayedSq;
        float SSGT;
        bool Display = false;
        public Level9(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 9 - I HATE THIS LEVEL";
            bobIt = new TextButton[5];
            this.rand = rand;
            bobIt[0] = new TextButton(new Vector2(-320, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "0", "⬜", new Vector2(16, 16));
            bobIt[1] = new TextButton(new Vector2(-192, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "1", "⬜", new Vector2(16, 16));
            bobIt[2] = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "2", "⬜", new Vector2(16, 16));
            bobIt[3] = new TextButton(new Vector2(64, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "3", "⬜", new Vector2(16, 16));
            bobIt[4] = new TextButton(new Vector2(192, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "4", "⬜", new Vector2(16, 16));
            bobIt[0].Text.ChangeColor(new Color[1] { Color.Orange });
            bobIt[1].Text.ChangeColor(new Color[1] { Color.DarkRed });
            bobIt[2].Text.ChangeColor(new Color[1] { Color.Green });
            bobIt[3].Text.ChangeColor(new Color[1] { Color.Blue });
            bobIt[4].Text.ChangeColor(new Color[1] { Color.Purple });
            for (int i = 0; i < bobIt.Length; i++) {
                bobIt[i].Click += BtnEvent;
            }
            Sequenz = string.Empty;
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            Marker = new TextBuilder[2];
            Marker[0] = new TextBuilder("Simon says!", new Vector2(-70, -132), new Vector2(16, 16), null, 0);
            Marker[0].Position = Vector2.Zero - Marker[0].rec.Size.ToVector2() / 2;
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
                    Marker[1].Position = Vector2.Zero - Marker[1].rec.Size.ToVector2() / 2;
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
                            mkc[i] = Color.Purple;;
                            break;
                    }
                }
                Marker[1] = new TextBuilder(mk, new Vector2(-40, 128), new Vector2(16, 16), mkc, 0);
                Marker[1].Position = Vector2.Zero - Marker[1].rec.Size.ToVector2() / 2;
                return;
            }
            CallFail(sender, e);
        }
        public override void Draw(SpriteBatch sp) {
            if (!(StartBtn is null))
                StartBtn.Draw(sp);
            else {
                if (!PlayingSequenz) {
                    Marker[1].Draw(sp);
                }
                for (int i = 0; i < bobIt.Length; i++) {
                    bobIt[i].Draw(sp);
                }
            }
            
            Marker[0].Draw(sp);
            mouseCursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            mouseCursor.Update(gt);
            base.Update(gt);

            Marker[0].Update(gt);

            if (!(StartBtn is null))
                StartBtn.Update(gt, mouseCursor.Hitbox[0]);
            else {

                if (!PlayingSequenz) {
                    Marker[1].Update(gt);
                } else {
                    SSGT += (float)gt.ElapsedGameTime.TotalMilliseconds;
                    while (SSGT > 500) {
                        SSGT -= 500;
                        bobIt[0].Text.ChangeColor(new Color[1] { Color.Orange });
                        bobIt[1].Text.ChangeColor(new Color[1] { Color.DarkRed });
                        bobIt[2].Text.ChangeColor(new Color[1] { Color.Green });
                        bobIt[3].Text.ChangeColor(new Color[1] { Color.Blue });
                        bobIt[4].Text.ChangeColor(new Color[1] { Color.Purple });
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
                                        bobIt[0].Text.ChangeColor(new Color[1] { Color.MonoGameOrange });
                                        break;
                                    case 1:
                                        bobIt[1].Text.ChangeColor(new Color[1] { Color.Red });
                                        break;
                                    case 2:
                                        bobIt[2].Text.ChangeColor(new Color[1] { Color.LightGreen });
                                        break;
                                    case 3:
                                        bobIt[3].Text.ChangeColor(new Color[1] { Color.LightBlue });
                                        break;
                                    case 4:
                                        bobIt[4].Text.ChangeColor(new Color[1] { Color.LightPink });
                                        break;
                                }
                                
                                PlayedSq++;
                            }
                        }
                        Display = !Display;
                    }
                }
                for (int i = 0; i < bobIt.Length; i++) {
                    bobIt[i].Update(gt, mouseCursor.Hitbox[0]);
                }
            }
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
        }
    }
}
