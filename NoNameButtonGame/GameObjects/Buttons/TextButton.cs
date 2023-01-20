﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.Input;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects;

public class TextButton : EmptyButton
{
    public TextBuilder Text { get; }
    public string Name { get; }

    public TextButton(Vector2 position, Vector2 size, string name, string text,
        Vector2 TextSize) : base(position, size)
    {
        Text = new TextBuilder(text, Position, TextSize, 0);
        Name = name;
    }

    public void Update(GameTime gt, Rectangle MousePos)
    {
        Text.ChangePosition(rectangle.Center.ToVector2() - Text.rectangle.Size.ToVector2() / 2);
        Text.Update(gt);
        base.Update(gt, MousePos);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        Text.Draw(spriteBatch);
    }
}