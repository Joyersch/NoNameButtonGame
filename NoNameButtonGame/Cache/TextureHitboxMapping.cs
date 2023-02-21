using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Cache;

public struct TextureHitboxMapping
{
    public Vector2 ImageSize;
    public Rectangle[] Hitboxes;
    public int AnimationsFrames;
    public bool? AnimationFromTop;
}