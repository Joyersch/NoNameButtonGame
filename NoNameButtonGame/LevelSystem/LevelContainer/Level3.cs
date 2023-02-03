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
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

class Level3 : SampleLevel
{
    private readonly Cursor cursor;
    private TextBuilder test;
    private TextButton generator;
    private Random random;
    private Rainbow rainbow;

    public Level3(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight,
        window, rand)
    {
        Name = "Level 3 - Tutorial time!";
        random = rand;
        cursor = new Cursor(Vector2.One);
        test = new TextBuilder("This is a sentence", new Vector2(-280,-32));
        generator = new TextButton(new Vector2(-32,0), "gen0", "generate");
        generator.ClickEventHandler += GeneratorOnClickEventHandler;
        rainbow = new Rainbow();
        rainbow.Increment = 64;
        test.ChangeColor(rainbow.GetColor(test.Length));
    }

    private void GeneratorOnClickEventHandler(object obj)
    {
        string text = string.Empty;

        for (int i = 0; i < test.Length; i++)
        {
            text += Letter.ReverseParse((Letter.Character) random.Next(0, 62)).ToString();
        }
        test.ChangeText(text);
        test.ChangeColor(rainbow.GetColor(test.Length));
    }

    public override void Update(GameTime gameTime)
    {
        cursor.Update(gameTime);
        cursor.Position = mousePosition - cursor.Size / 2;
        generator.Update(gameTime, cursor.rectangle);
        test.Update(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        test.Draw(spriteBatch);
        generator.Draw(spriteBatch);
        cursor.Draw(spriteBatch);
    }
}