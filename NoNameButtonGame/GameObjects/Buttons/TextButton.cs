using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Cache;
using NoNameButtonGame.Input;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects.Buttons;

public class TextButton : EmptyButton, IMoveable
{
    public TextBuilder Text { get; }
    public string Name { get; }

    public TextButton(Vector2 position, string name, string text) : this(position, DefaultSize, name, text)
    {
    }

    public TextButton(Vector2 position, float scale, string name, string text) : this(position, DefaultSize * scale,
        name, text, DefaultTextSize * scale)
    {
    }

    public TextButton(Vector2 position, Vector2 canvas, string name, string text) : this(position, canvas, name, text,
        DefaultTextSize)
    {
    }

    public TextButton(Vector2 position, Vector2 canvas, string name, string text, Vector2 textSize) : this(position, canvas,
        name,
        text, textSize, 1)
    {
    }

    public TextButton(Vector2 position, Vector2 canvas, string name, string text,
        Vector2 textSize, int spacing) : base(position, canvas)
    {
        Text = new TextBuilder(text, Position, textSize, spacing);
        Text.ChangePosition(Rectangle.Center.ToVector2() - Text.Rectangle.Size.ToVector2() / 2);
        Name = name;
    }

    public override bool Move(Vector2 newPosition)
    {
        var success = base.Move(newPosition);
        UpdateRectangle();
        Text.ChangePosition(Rectangle.Center.ToVector2() - Text.Rectangle.Size.ToVector2() / 2);
        return success;
    }

    public override void Update(GameTime gt, Rectangle MousePos)
    {
        Text.Update(gt);
        base.Update(gt, MousePos);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        Text.Draw(spriteBatch);
    }


    public new static Vector2 DefaultSize => new Vector2(128, 64);
    public static Vector2 DefaultTextSize => new Vector2(16, 16);
}