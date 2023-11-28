using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;

namespace NoNameButtonGame.GameObjects;

public class Dot : GameObject
{
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;
    public new static Texture2D DefaultTexture;
    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(1, 1),
        Hitboxes = Array.Empty<Rectangle>()
    };

    public Dot(Vector2 position, Vector2 size) : base(position, size, DefaultTexture, DefaultMapping)
    {
    }
}