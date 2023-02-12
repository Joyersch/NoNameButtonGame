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

internal class Level7 : SampleLevel
{
    private readonly EmptyButton button;
    private readonly Cursor cursor;
    private readonly TextBuilder[] Infos;
    private readonly GlitchBlockCollection[] WallLeft;
    private readonly GlitchBlockCollection[] WallRight;
    private readonly GlitchBlockCollection[] Blocks;
    private readonly int WallLength = 10;
    private float GT;

    public Level7(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight,
        window, rand)
    {
        Name = "Level 7 - WHAT OUT WHAT OUT OOHHH";
        button = new WinButton(new Vector2(-256, -0), new Vector2(128, 64));
        button.ClickEventHandler += Finish;
        cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        Infos = new TextBuilder[2];
        Infos[0] = new TextBuilder("wow you did it!", new Vector2(120, -132), new Vector2(8, 8), null, 0);
        Infos[1] = new TextBuilder("gg!", new Vector2(120, -100), new Vector2(8, 8), null, 0);
        WallLeft = new GlitchBlockCollection[WallLength];
        WallRight = new GlitchBlockCollection[WallLength];
        List<GlitchBlockCollection> Walls = new List<GlitchBlockCollection>();
        for (int i = 0; i < WallLength; i++)
        {
            WallLeft[i] = new GlitchBlockCollection(new Vector2(96, 512 * i), new Vector2(416, 512));
            WallRight[i] = new GlitchBlockCollection(new Vector2(-512, 512 * i), new Vector2(416, 512));
            WallRight[i].EnterEventHandler += Fail;
            WallLeft[i].EnterEventHandler += Fail;
        }

        for (int i = 0; i < (WallLength - 1) * 8 / 2; i++)
        {
            int c = rand.Next(0, 3);
            if (c != 0)
                Walls.Add(new GlitchBlockCollection(new Vector2(-96, 512 + 128 * i), new Vector2(64, 64)));
            if (c != 1)
                Walls.Add(new GlitchBlockCollection(new Vector2(-32, 512 + 128 * i), new Vector2(64, 64)));
            if (c != 2)
                Walls.Add(new GlitchBlockCollection(new Vector2(32, 512 + 128 * i), new Vector2(64, 64)));

            // Walls.Add(new Laserwall(new Vector2(-96 + rand.Next(0, 3) * 64, 512 + 128 * i), new Vector2(64, 64), Globals.Content.GetTHBox("zonenew")));
        }

        Blocks = Walls.ToArray();
        for (int i = 0; i < Blocks.Length; i++)
        {
            Blocks[i].EnterEventHandler += Fail;
        }
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        button.Draw(spriteBatch);
        for (int i = 0; i < Infos.Length; i++)
        {
            Infos[i].Draw(spriteBatch);
        }

        for (int i = 0; i < WallLength; i++)
        {
            if (WallLeft[i].Rectangle.Intersects(cameraRectangle))
                WallLeft[i].Draw(spriteBatch);
            if (WallRight[i].Rectangle.Intersects(cameraRectangle))
                WallRight[i].Draw(spriteBatch);
        }

        for (int i = 0; i < Blocks.Length; i++)
        {
            if (Blocks[i].Rectangle.Intersects(cameraRectangle))
                Blocks[i].Draw(spriteBatch);
        }

        cursor.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        cursor.Update(gameTime);
        base.Update(gameTime);
        for (int i = 0; i < Infos.Length; i++)
        {
            Infos[i].Update(gameTime);
        }

        GT += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        while (GT > 8)
        {
            GT -= 8;
            for (int i = 0; i < WallLength; i++)
            {
                WallLeft[i].Move(new Vector2(0, -2));
                WallRight[i].Move(new Vector2(0, -2));
            }

            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].Move(new Vector2(0, -2));
            }
        }

        cursor.Position = mousePosition - cursor.Size / 2;
        button.Update(gameTime, cursor.Hitbox[0]);
        for (int i = 0; i < WallLength; i++)
        {
            WallLeft[i].Update(gameTime, cursor.Hitbox[0]);
            WallRight[i].Update(gameTime, cursor.Hitbox[0]);
        }

        for (int i = 0; i < Blocks.Length; i++)
        {
            Blocks[i].Update(gameTime, cursor.Hitbox[0]);
        }
    }
}