using System;
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
    TextBuilder textContainer;

    public TextBuilder Text => textContainer;

    public TextButton(Vector2 Pos, Vector2 Size, string Name, string Text,
        Vector2 TextSize) : base(Pos, Size)
    {
        textContainer = new TextBuilder(Text, Position, TextSize, null, 0);
        textContainer.ChangeText(Text);
        this.Name = Name;
    }

    public void Update(GameTime gt, Rectangle MousePos)
    {
        textContainer.ChangePosition(rectangle.Center.ToVector2() - textContainer.rectangle.Size.ToVector2() / 2);
        textContainer.Update(gt);
        base.Update(gt, MousePos);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        textContainer.Draw(spriteBatch);
    }
}