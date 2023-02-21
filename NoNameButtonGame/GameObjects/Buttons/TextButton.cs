using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Text;
using NoNameButtonGame.GameObjects.Texture;

namespace NoNameButtonGame.GameObjects.Buttons;

public class TextButton : EmptyButton
{
    public TextBuilder Text { get; }
    public string Name { get; }
    public new static Vector2 DefaultSize => new Vector2(128, 64);
    public static Vector2 DefaultTextSize => new Vector2(16, 16);

    public TextButton(Vector2 position, string name, string text) : this(position, DefaultSize, name, text)
    {
    }

    public TextButton(Vector2 position, float scale, string name, string text) : this(position, DefaultSize * scale,
        name, text, DefaultTextSize * scale)
    {
    }

    public TextButton(Vector2 position, Vector2 size, string name, string text) : this(position, size, name, text,
        DefaultTextSize)
    {
    }

    public TextButton(Vector2 position, Vector2 size, string name, string text, Vector2 textSize) :
        this(position, size, name, text, textSize, 1)
    {
    }

    public TextButton(Vector2 position, Vector2 size, string name, string text,
        Vector2 textSize, int spacing) : this(position, size, name, text, textSize, spacing, DefaultTexture, DefaultMapping)
    {
    }

    public TextButton(Vector2 position, Vector2 size, string name, string text,
        Vector2 textSize, int spacing, Texture2D texture, TextureHitboxMapping mapping) :
        base(position, size, texture, mapping)
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

    public override void Update(GameTime gameTime, Rectangle mousePos)
    {
        Text.Update(gameTime);
        base.Update(gameTime, mousePos);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        Text.Draw(spriteBatch);
    }
}