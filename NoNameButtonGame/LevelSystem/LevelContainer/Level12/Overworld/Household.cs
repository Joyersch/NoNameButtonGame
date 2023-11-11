using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Management;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level12.Overworld;

public class Household : IManageable
{
    private readonly Random _random;
    private House _house;
    private List<Human> _humans;

    public Household(Vector2 position, float scale, int humans, Random random)
    {
        _random = random;
        _house = new House(position, scale);
        _humans = new List<Human>();
        
        for (int i = 0; i < humans; i++)
        {
            var humanOffset = new Vector2(_random.Next(0, 8), _random.Next(0, 8));
            _humans.Add(new Human(position + humanOffset, scale, random));
        }
    }

    public Rectangle Rectangle => _house.Rectangle;

    public void Update(GameTime gameTime)
    {
        _house.Update(gameTime);
        foreach (var human in _humans)
            human.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _house.Draw(spriteBatch);
        foreach (var human in _humans)
            human.Draw(spriteBatch);
    }
}