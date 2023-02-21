using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Cache;
using NoNameButtonGame.GameObjects.Texture;

namespace NoNameButtonGame.GameObjects.Buttons.TexturedButtons;

public class MiniTextButton : TextButton
{
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize * 4;
    public new static Vector2 DefaultTextSize => new Vector2(16, 16);
    
    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(16, 8),
        Hitboxes = new[]
        {
            new Rectangle(0, 1, 16, 6),
            new Rectangle(1, 0, 14, 8)
        }
    };
    
    public MiniTextButton(Vector2 position, string name, string text) : this(position, DefaultSize, name, text)
    {
    }

    public MiniTextButton(Vector2 position, float scale, string name, string text) : this(position,
        DefaultSize * scale, name, text)
    {
    }

    public MiniTextButton(Vector2 position, Vector2 size, string name, string text) : this(position, size, name, text,
        DefaultTextSize)
    {
    }

    public MiniTextButton(Vector2 position, Vector2 size, string name, string text, Vector2 textSize) : this(position,
        size, name, text, textSize, 1)
    {
    }

    public MiniTextButton(Vector2 position, Vector2 size, string name, string text, Vector2 textSize, int spacing) :
        this(position, size, name, text, textSize, spacing, DefaultTexture, DefaultMapping)
    {
    }

    public MiniTextButton(Vector2 position, Vector2 size, string name, string text, Vector2 textSize, int spacing,
        Texture2D texture, TextureHitboxMapping mapping) : base(position, size, name, text, textSize, spacing, texture,
        mapping)
    {
    }
}