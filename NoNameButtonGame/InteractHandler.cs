using System.Collections.Generic;
using System.Linq;
using Joyersch.Monogame;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public sealed class InteractHandler : IInteractable
{
    private List<(int zIndex, IInteractable interactable)> _interactables;

    public Rectangle[] Hitbox => [];

    public InteractHandler()
    {
        _interactables = new();
    }

    public void AddInteractable(IInteractable interactable, int zIndex)
        => _interactables.Add((zIndex, interactable));

    public bool UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        bool @return = false;
        foreach (var element in _interactables.OrderByDescending(i => i.zIndex))
        {
            @return |= element.interactable.UpdateInteraction(gameTime, toCheck);
            if (@return)
                break;
        }

        return @return;
    }

    public void Clear()
        => _interactables.Clear();
}