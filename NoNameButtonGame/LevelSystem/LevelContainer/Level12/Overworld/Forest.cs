using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level12.Overworld;

public class Forest : ConnectedGameObject
{
    public Forest(int variation = 0) : this(Vector2.Zero, variation)
    {
    }

    public Forest(Vector2 position, int variation) : this(position, 1F, variation)
    {
    }

    public Forest(Vector2 position, float scale, int variation) : this(position, scale * DefaultSize, variation)
    {
    }

    public Forest(Vector2 position, Vector2 size, int variation) : this(position, size, variation, DefaultTexture, DefaultMapping)
    {
    }

    public Forest(Vector2 position, Vector2 size, int variation, Texture2D texture, TextureHitboxMapping mapping) : base(position, size, variation, texture, mapping)
    {
        DrawColor = Color.Green;
    }

    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;

    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(32, 32),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 32, 32)
        }
    };
    
    
}