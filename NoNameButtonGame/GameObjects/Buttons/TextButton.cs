using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.Buttons;

public class TextButton : EmptyButton
{
    public TextSystem.Text Text { get; }
    public string Name { get; }
    public new static Vector2 DefaultSize => new Vector2(128, 64);
    public static Vector2 DefaultTextSize => new Vector2(16, 16);

    public TextButton(string text) : this(Vector2.Zero, string.Empty, text)
    {
    }


    public TextButton(string text, float scale) : this(string.Empty, text, scale)
    {
    }

    public TextButton(string text, float scale, float textScale) :
        this(Vector2.Zero, DefaultSize * scale, string.Empty, text, DefaultTextSize * textScale)
    {
    }

    public TextButton(string text, string name) : this(Vector2.Zero, name, text)
    {
    }

    public TextButton(string text, string name, float scale) : this(Vector2.Zero, scale, name, text)
    {
    }

    public TextButton(Vector2 position, string text) : this(position, string.Empty, text)
    {
    }

    public TextButton(Vector2 position, float scale, string text) : this(position, scale, string.Empty, text)
    {
    }

    public TextButton(Vector2 position, string name, string text) : this(position, 1, name, text)
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
        Vector2 textSize, int spacing) : this(position, size, name, text, textSize, spacing, DefaultTexture,
        DefaultMapping)
    {
    }

    public TextButton(Vector2 position, Vector2 size, string name, string text,
        Vector2 textSize, int spacing, Texture2D texture, TextureHitboxMapping mapping) :
        base(position, size, texture, mapping)
    {
        Text = new TextSystem.Text(text, Position, textSize, spacing);
        Text.Move(Rectangle.Center.ToVector2() - Text.Rectangle.Size.ToVector2() / 2);
        Name = name;
    }

    public override void Move(Vector2 newPosition)
    {
        base.Move(newPosition);
        UpdateRectangle();
        Text.Move(Rectangle.Center.ToVector2() - Text.Rectangle.Size.ToVector2() / 2);
    }

    public override void Update(GameTime gameTime)
    {
        Text.Update(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        Text.Draw(spriteBatch);
    }
}