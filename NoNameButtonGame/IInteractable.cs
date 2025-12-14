using Joyersch.Monogame;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public interface IInteractable : IHitbox
{
    public bool UpdateInteraction(GameTime gameTime, IHitbox toCheck);
}