using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Color;
using NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class OverworldCollection : IManageable, IInteractable
{
    private readonly Random _random;
    private readonly Camera _camera;
    private readonly List<IManageable> _overworld;

    public bool HasFullyGenerated { get; private set; }
    public bool CastleOnScreen { get; private set; }

    private IEnumerable<Village> _villagesView => _overworld.OfType<Village>().OrderBy(v => v.Houses);


    private Vector2 _bounds;

    public int UpdatesRequired = 0;
    public int UpdatesCurrent => _onUpdate;
    private int _onUpdate = 0;

    public Rectangle Rectangle =>
        new Rectangle(-(int)_bounds.X * 32, -(int)_bounds.Y * 32, (int)_bounds.X * 2 * 32, (int)_bounds.Y * 2 * 32);

    public event Action<ILocation> Interaction;

    private enum Tile
    {
        Forest = 0,
        Castle = 1,
        Village = 2,
        Path = 3
    }

    public OverworldCollection(Random random, Camera camera, Vector2 bounds)
    {
        _random = random;
        _camera = camera;
        _bounds = bounds;
        _overworld = new List<IManageable>();
        UpdatesRequired = (int)(_bounds.X * _bounds.Y);
    }

    public void Update(GameTime gameTime)
    {
        if (!HasFullyGenerated)
        {
            if (_onUpdate >= UpdatesRequired)
            {
                HasFullyGenerated = true;
                // forests.ForEach(f => f.SetTextureLocation(forests));
                // _overworld.AddRange(forests);
            }
            return;
        }

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

    public bool Generate()
    {
        _onUpdate++;
        if (_onUpdate == 1)
        {
            var castle = new Castle(Vector2.Zero, 1F, "castle");
            castle.Interacted += () => Interaction?.Invoke(castle);
            _overworld.Add(castle);
            return false;
        }

        int x = _onUpdate % (int)_bounds.X;
        int y = _onUpdate / (int)_bounds.Y;
        int rand = _random.Next(0, 1000);

        if (rand < 900)
            return false;

        float positionX = x * 32 - _bounds.X / 2 * 32;
        float positionY = y * 32 - _bounds.Y / 2 * 32;
        var position = new Vector2(positionX, positionY);

        if (rand < 999)
        {
            var forest = new Forest(position, _random.Next(0, 2));
            //forests.Add(forest);
        }
        else
        {
            var village = new Village(position, _random, new Guid().ToString());
            _overworld.Add(village);
        }

        return _onUpdate >= UpdatesRequired;
    }

    private string GetCastleName()
    {
        return "castle";
    }

    public Guid GetCastle()
        => ((Castle)_overworld.First(m => m is Castle)).GetGuid();
}