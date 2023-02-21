using Microsoft.Xna.Framework;

namespace NoNameButtonGame.GameObjects.Texture;

public struct TextureHitboxMapping
{
    public Vector2 ImageSize;
    public Rectangle[] Hitboxes;
    public int AnimationsFrames;
    public bool? AnimationFromTop;
}