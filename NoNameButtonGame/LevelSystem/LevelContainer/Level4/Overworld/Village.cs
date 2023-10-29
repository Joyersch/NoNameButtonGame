using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Logic;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld;

public class Village : GameObject, IManageable, IInteractable, ILocation
{
    public readonly Vector2 Position;
    private readonly Random _random;
    private readonly List<Household> _houses;

    private bool _isHover;
    private string _name;
    private Guid _guid;

    public int Houses => _houses.Count;
    
    public event Action Interacted;

    public Village(Vector2 position, Random random, string name)
    {
        Position = position;
        _random = random;
        _name = name;
        _houses = new List<Household>();
        _guid = Guid.NewGuid();

        int houses = _random.Next(2, 11);
        for (int house = 0; house < houses; house++)
        {
            var houseOffset = new Vector2(_random.Next(-30, 30), _random.Next(-30, 30));
            var people = _random.Next(2, 5);
            _houses.Add(new Household(position + houseOffset, 1F, people, random));
        }
    }

    public Rectangle Rectangle =>
        _houses.Aggregate(_houses[0].Rectangle, (current, h) => Rectangle.Union(current, h.Rectangle));

    public override void Update(GameTime gameTime)
    {
        if (_isHover && InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
            Interacted?.Invoke();
        
        foreach (var house in _houses)
            house.Update(gameTime);
        base.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var house in _houses)
            house.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _isHover = toCheck.Hitbox.Any(c => c.Intersects(Rectangle.ExtendFromCenter(1.5F)));
    }

    public string GetName()
        => _name;

    public Guid GetGuid()
        => _guid;
}