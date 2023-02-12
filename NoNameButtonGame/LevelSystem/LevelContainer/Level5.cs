using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Cache;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level5 : SampleLevel
{
    private readonly HoldButton button;
    private readonly Cursor cursor;
    private readonly TextBuilder[] Infos;
    private readonly LockButton lockbutton;
    private readonly Random rand;
    private float gameTime;

    public Level5(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth,
        defaultHeight, window, rand)
    {
        Name = "Level 5 - MORE BUTTONS!";
        this.rand = rand;
        button = new HoldButton(new Vector2(-220, -100), new Vector2(128, 64))
        {
            EndHoldTime = 6900
        };
        button.ClickEventHandler += EmptyBtnEvent;
        cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        Infos = new TextBuilder[2];
        Infos[0] = new TextBuilder("<-- Hold this button till the timer runs out!", new Vector2(-64, -72),
            new Vector2(8, 8), null, 0);
        Infos[1] = new TextBuilder("<-- This one will be unlocked then", new Vector2(-64, 32), new Vector2(8, 8),
            null, 0);
        for (int i = 0; i < Infos.Length; i++)
        {
            Color[] c = new Color[Infos[i].Text.Length];
            for (int a = 0; a < Infos[i].Text.Length; a++)
            {
                c[a] = new Color(rand.Next(64, 255), rand.Next(64, 255), rand.Next(64, 255));
            }

            Infos[i].ChangeColor(c);
        }

        lockbutton = new LockWinButton(new Vector2(-220, 0), new Vector2(128, 64), true);
        lockbutton.ClickEventHandler += Finish;
    }

    private void EmptyBtnEvent(object sender)
        => lockbutton.Unlock();
    
    public override void Update(GameTime gameTime)
    {
        this.gameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        while (this.gameTime > 512)
        {
            this.gameTime -= 512;
            for (int i = 0; i < Infos.Length; i++)
            {
                Color[] c = new Color[Infos[i].Text.Length];
                for (int a = 0; a < Infos[i].Text.Length; a++)
                {
                    c[a] = new Color(rand.Next(63, 255), rand.Next(63, 255), rand.Next(63, 255));
                }

                Infos[i].ChangeColor(c);
            }
        }

        cursor.Update(gameTime);
        base.Update(gameTime);
        for (int i = 0; i < Infos.Length; i++)
        {
            Infos[i].Update(gameTime);
        }

        cursor.Position = mousePosition - cursor.Canvas / 2;
        button.Update(gameTime, cursor.Hitbox[0]);
        lockbutton.Update(gameTime, cursor.Hitbox[0]);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        button.Draw(spriteBatch);
        for (int i = 0; i < Infos.Length; i++)
        {
            Infos[i].Draw(spriteBatch);
        }

        lockbutton.Draw(spriteBatch);
        cursor.Draw(spriteBatch);
    }
}