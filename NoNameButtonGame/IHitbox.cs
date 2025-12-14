using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public interface IHitbox
{
    public Rectangle[] Hitbox { get; }
}