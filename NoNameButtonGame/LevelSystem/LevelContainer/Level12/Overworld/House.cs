using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level12.Overworld;

public class House : GameObject
{
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;
    public new static Texture2D DefaultTexture;
    
    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(16, 16),
        Hitboxes = new[]
        {
            new Rectangle(1, 2, 21, 13)
        }
    };
    
    public House(Vector2 position, float scale) : base(position, DefaultSize * scale,
        DefaultTexture,
        DefaultMapping)
    {
        DrawColor = Color.Gray;
    }
}