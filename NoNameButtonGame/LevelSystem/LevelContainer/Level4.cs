﻿using System;
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
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level4 : SampleLevel
{
    private readonly EmptyButton button;
    private readonly Cursor cursor;
    private readonly TextBuilder[] Infos;
    private readonly Laserwall wall;

    public Level4(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight,
        window, rand)
    {
        Name = "Level 4 - Bug? No its a Feature!";
        button = new WinButton(new Vector2(-256, -0), new Vector2(128, 64));
        button.ClickEventHandler += Finish;
        cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        Infos = new TextBuilder[2];
        Infos[0] = new TextBuilder("Thin walls can be penetrated!", new Vector2(80, -132), new Vector2(8, 8), null, 0);
        Infos[1] = new TextBuilder("Just move fast enough!", new Vector2(80, -100), new Vector2(8, 8), null, 0);
        wall = new Laserwall(new Vector2(-40, -300), new Vector2(24, 1024));
        wall.EnterEventHandler += Fail;
    }

    public override void Update(GameTime gameTime)
    {
        cursor.Update(gameTime);
        base.Update(gameTime);
        for (int i = 0; i < Infos.Length; i++)
        {
            Infos[i].Update(gameTime);
        }

        cursor.Position = mousePosition - cursor.Size / 2;
        button.Update(gameTime, cursor.Hitbox[0]);
        wall.Update(gameTime, cursor.Hitbox[0]);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        button.Draw(spriteBatch);
        for (int i = 0; i < Infos.Length; i++)
        {
            Infos[i].Draw(spriteBatch);
        }

        wall.Draw(spriteBatch);
        cursor.Draw(spriteBatch);
    }
}