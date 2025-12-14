using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Collision;

public sealed class EmptyHitbox : IHitbox
{
    public Rectangle[] Hitbox { get; } =  new Rectangle[0];
}