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
    public long UpdatesCurrent => _onUpdate;
    private long _onUpdate = 0;

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
        UpdatesRequired = 100000000;
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
        return _onUpdate >= UpdatesRequired;
    }

    private string GetCastleName()
    {
        return "castle";
    }

    public Guid GetCastle()
        => ((Castle)_overworld.First(m => m is Castle)).GetGuid();
}