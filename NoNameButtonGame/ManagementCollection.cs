using System.Collections.Generic;
using Joyersch.Monogame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame;

public sealed class ManagementCollection : List<IManageable>, IManageable, IInteractable
{
    public Rectangle Rectangle => Rectangle.Empty;

    public Rectangle[] Hitbox => _hitbox.ToArray();
    private List<Rectangle> _hitbox = [];

    public void Update(GameTime gameTime)
    {
        _hitbox.Clear();
        foreach (var manageable in this)
        {
            manageable.Update(gameTime);
            if (manageable is IInteractable interactable)
                _hitbox.AddRange(interactable.Hitbox);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var manageable in this)
            manageable.Draw(spriteBatch);
    }

    public bool UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        bool @return = false;
        foreach (var manageable in this)
        {
            if (manageable is IInteractable interactable)
                @return |= interactable.UpdateInteraction(gameTime, toCheck);
        }

        return @return;
    }


}