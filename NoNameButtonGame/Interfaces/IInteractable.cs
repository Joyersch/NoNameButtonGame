using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Interfaces;

public interface IInteractable
{
    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck);
}