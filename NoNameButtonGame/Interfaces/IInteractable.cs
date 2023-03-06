using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Interfaces;

public interface IInteractable
{
    public void Update(GameTime gameTime, IHitbox toCheck);
}