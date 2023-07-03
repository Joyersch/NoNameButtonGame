using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class OverworldCollection : List<IManageable>, IManageable
{
    private readonly Random _random;
    private readonly Camera _camera;

    private Vector2 _bounds => new Vector2(2000, 2000);
    public Rectangle Rectangle => new Rectangle(-(int)_bounds.X, -(int)_bounds.Y, (int)_bounds.X * 2, (int)_bounds.Y * 2);

    public OverworldCollection(Random random, Camera camera)
    {
        _random = random;
        _camera = camera;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var obj in this)
            obj.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var obj in this)
            if (obj.Rectangle.Intersects(_camera.Rectangle))
                obj.Draw(spriteBatch);
    }

    public void GenerateVillage(int amount)
    {
        for (int villageNumber = 0; villageNumber < amount; villageNumber++)
        {
            var location = new Vector2(_random.Next(-(int)_bounds.X, (int)_bounds.X), _random.Next(-(int)_bounds.Y, (int)_bounds.Y));
            Log.Write($"Village at:{location}");
            var houses = _random.Next(2, 11);
            for (int house = 0; house < houses; house++)
            {
                var houseOffset = new Vector2(_random.Next(-30, 30), _random.Next(-30, 30));
                Add(new House(location + houseOffset, 1F));
                var people = _random.Next(2, 5);
                for (int person = 0; person < people; person++)
                {
                    var humanOffset = new Vector2(_random.Next(0, 8), _random.Next(0, 8));
                    Add(new Human(location + houseOffset + humanOffset + new Vector2(0, 5), 1F, _random));
                }
            }
        }
    }

    public void GenerateTrees(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var location = new Vector2(_random.Next(-(int)_bounds.X, (int)_bounds.X), _random.Next(-(int)_bounds.Y, (int)_bounds.Y));
            Add(_random.Next(0, 2) == 0
                ? new SmallTree(location, 1F)
                : new BigTree(location, 1F));
        }
    }

    public void SetColor()
    {
        foreach (var game in this)
        {
            if (game is BigTree bigTree)
                bigTree.DrawColor = Color.DarkGreen;
            if (game is SmallTree smallTree)
                smallTree.DrawColor = Color.Green;
            if (game is House house)
                house.DrawColor = Color.Gray;
            if (game is Human human)
                human.DrawColor = Color.Pink;
        }
    }
}