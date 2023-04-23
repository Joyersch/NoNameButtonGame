using Microsoft.Xna.Framework;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects;

public class EmptyHitbox : IHitbox
{
    public Rectangle[] Hitbox { get; } =  new Rectangle[0];
}