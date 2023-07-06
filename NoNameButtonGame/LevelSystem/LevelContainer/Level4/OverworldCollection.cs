using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class OverworldCollection : IManageable, IInteractable
{
    private readonly Random _random;
    private readonly Camera _camera;
    private readonly List<IManageable> _overworld;
    public bool CastleOnScreen { get; private set; }

    private IEnumerable<Village> _villagesView => _overworld.OfType<Village>().OrderBy(v => v.Houses);

    private Vector2 _bounds => new Vector2(2000, 2000);

    public Rectangle Rectangle =>
        new Rectangle(-(int) _bounds.X, -(int) _bounds.Y, (int) _bounds.X * 2, (int) _bounds.Y * 2);

    public event Action<ILocation> Interaction;

    public OverworldCollection(Random random, Camera camera)
    {
        _random = random;
        _camera = camera;
        _overworld = new List<IManageable>();
    }

    public void Update(GameTime gameTime)
    {
        CastleOnScreen = false;
        foreach (var obj in _overworld)
        {
            if (obj.Rectangle.Intersects(_camera.Rectangle))
            {
                obj.Update(gameTime);

                if (obj is Castle)
                    CastleOnScreen = true;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var obj in _overworld)
            if (obj.Rectangle.Intersects(_camera.Rectangle))
                obj.Draw(spriteBatch);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        foreach (var obj in _overworld)
        {
            if (obj is IInteractable interactable)
                interactable.UpdateInteraction(gameTime, toCheck);
        }
    }

    public void GenerateVillages(int amount)
    {
        for (int villageNumber = 0; villageNumber < amount; villageNumber++)
        {
            var village = new Village(GetOuterLocation(100F), _random, villageNumber.ToString());
            village.Interacted += () => Interaction?.Invoke(village);
            _overworld.Add(village);
        }
            
    }

    public void GenerateTrees(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var location = new Vector2(_random.Next(-(int) (_bounds.X + 320), (int) (_bounds.X + 320)),
                _random.Next(-(int) (_bounds.Y + 200), (int) (_bounds.Y + 200)));
            _overworld.Add(_random.Next(0, 2) == 0
                ? new SmallTree(location, 1F)
                : new BigTree(location, 1F));
        }
    }

    public Guid GenerateCastle()
    {
        var castle = new Castle(GetOuterLocation(200F), 1F, "castle");
        castle.Interacted += () => Interaction?.Invoke(castle);
        
        _overworld.Add(castle);
        return castle.GetGuid();
    }

    private Vector2 GetOuterLocation(float range)
    {
        Vector2 location;
        do
        {
            location = new Vector2(_random.Next(-(int) _bounds.X, (int) _bounds.X),
                _random.Next(-(int) _bounds.Y, (int) _bounds.Y));
        } while (!(location.X < -600 || location.X > 600 || location.Y > 400 || location.Y < -400) && !IsAroundAVillage(location, range));

        return location;
    }

    private bool IsAroundAVillage(Vector2 position, float distance)
        => _villagesView.Any(v => Vector2.Distance(v.Position, position) <= distance);
}