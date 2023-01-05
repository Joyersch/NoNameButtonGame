using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.BeforeMaths;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level20 : SampleLevel
    {

        readonly TextButton[] bobIt;
        readonly Cursor mouseCursor;
        readonly TextBuilder[] Marker;
        AwesomeButton startButton;
        readonly Random globalRandom;

        readonly int sequenzLength = 7;
        bool PlayingSequenz = false;
        string CurrentSequenz;
        string Sequenz;
        int currentSequenzAmmount = 1;
        int playedSequenz;
        
        float SSGT;
        bool Display = false;
        public Level20(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 20 - I HATE THIS LEVEL MORE";
            bobIt = new TextButton[5];
            this.globalRandom = rand;
            bobIt[0] = new TextButton(new Vector2(-320, -32), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "0", "⬜", new Vector2(16, 16));
            bobIt[1] = new TextButton(new Vector2(-192, -32), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "1", "⬜", new Vector2(16, 16));
            bobIt[2] = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "2", "⬜", new Vector2(16, 16));
            bobIt[3] = new TextButton(new Vector2(64, -32), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "3", "⬜", new Vector2(16, 16));
            bobIt[4] = new TextButton(new Vector2(192, -32), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "4", "⬜", new Vector2(16, 16));
            bobIt[0].Text.ChangeColor(new Color[1] { Color.Orange });
            bobIt[1].Text.ChangeColor(new Color[1] { Color.DarkRed });
            bobIt[2].Text.ChangeColor(new Color[1] { Color.Green });
            bobIt[3].Text.ChangeColor(new Color[1] { Color.Blue });
            bobIt[4].Text.ChangeColor(new Color[1] { Color.Purple });
            for (int i = 0; i < bobIt.Length; i++) {
                bobIt[i].Click += BtnEvent;
            }
            Sequenz = string.Empty;
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            Marker = new TextBuilder[2];
            Marker[0] = new TextBuilder("Simon says!", new Vector2(-70, -132), new Vector2(16, 16), null, 0);
            Marker[0].Position = Vector2.Zero - Marker[0].rec.Size.ToVector2() / 2;
            Marker[1] = new TextBuilder("", new Vector2(-40, 128), new Vector2(16, 16), null, 0);
            startButton = new AwesomeButton(new Vector2(-80, -32), new Vector2(160, 64), Globals.Content.GetTHBox("startbutton"));
            startButton.Click += StartEvent;
        }

        private void StartEvent(object sender, EventArgs e) {
            PlayingSequenz = true;
            startButton = null;
        }

        private void BtnEvent(object sender, EventArgs e) {
            if (PlayingSequenz)
                return;
            CurrentSequenz += (sender as TextButton).Name;
            if (CurrentSequenz == Sequenz) {
                if (sequenzLength == CurrentSequenz.Length)
                    CallFinish(this, e);
                else {
                    currentSequenzAmmount++;
                    CurrentSequenz = string.Empty;
                    PlayingSequenz = true;
                    playedSequenz = 0;
                    Marker[1] = new TextBuilder("", new Vector2(-40, 128), new Vector2(16, 16), null, 0);
                    Marker[1].Position = Vector2.Zero - Marker[1].rec.Size.ToVector2() / 2;
                }
                return;
            }
            if (Sequenz.Substring(0, currentSequenzAmmount).StartsWith(CurrentSequenz)) {
                string mk = string.Empty;
                Color[] mkc = new Color[currentSequenzAmmount];
                for (int i = 0; i < currentSequenzAmmount; i++) {
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
                Marker[1].Position = Vector2.Zero - Marker[1].rec.Size.ToVector2() / 2;
                return;
            }
            CallFail(sender, e);
        }
        public override void Draw(SpriteBatch sp) {
            if (!(startButton is null))
                startButton.Draw(sp);
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

            if (!(startButton is null))
                startButton.Update(gt, mouseCursor.Hitbox[0]);
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
                            if (playedSequenz >= currentSequenzAmmount) {
                                PlayingSequenz = false;

                            } else {
                                int r;
                                if (playedSequenz + 1 <= Sequenz.Length && Sequenz.Length != 0) {
                                    r = int.Parse(Sequenz.ToCharArray()[playedSequenz].ToString());
                                } else {
                                    r = globalRandom.Next(0, 5);
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

                                playedSequenz++;
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
