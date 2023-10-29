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
    private GameObject _worldIndicator;

    public bool HasFullyGenerated { get; private set; }
    public bool CastleOnScreen { get; private set; }

    // ToDo: still needed?
    private IEnumerable<Village> _villagesView => _overworld.OfType<Village>().OrderBy(v => v.Houses);


    private Vector2 _bounds;

    public long GenerateRequired { get; private set; } = 0;

    public long GenerateCurrent { get; private set; }

    public string GenerateGoal { get; private set; }

    private LoadingState _generateProgress = LoadingState.Start;

    public Rectangle Rectangle =>
        new Rectangle(-(int)_bounds.X * 32, -(int)_bounds.Y * 32, (int)_bounds.X * 2 * 32, (int)_bounds.Y * 2 * 32);

    public event Action<ILocation> Interaction;

    public enum LoadingState
    {
        Start = -1,
        Villages = 0,
        Castle = 1,
        Paths = 2,
        Forests = 3,
        Done = 4,
    }

    public OverworldCollection(Random random, Camera camera, Vector2 bounds)
    {
        _random = random;
        _camera = camera;
        _bounds = bounds;
        _overworld = new List<IManageable>();
        GenerateGoal = _generateProgress.ToString();
        _worldIndicator = new GameObject(Rectangle.Location.ToVector2(), Rectangle.Size.ToVector2());
        Log.Write($"Bounds:{Rectangle}");
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
        _worldIndicator.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var obj in _overworld)
            if (obj.Rectangle.Intersects(_camera.Rectangle))
                obj.Draw(spriteBatch);
        _worldIndicator.Draw(spriteBatch);
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
        switch (_generateProgress)
        {
            case LoadingState.Villages:
                GenerateVillages();
                break;
            case LoadingState.Castle:
                GenerateCastle();
                break;
            case LoadingState.Paths:
                GeneratePaths();
                break;
            case LoadingState.Forests:
                GenerateForests();
                break;
        }

        if (GenerateCurrent <= GenerateRequired &&
            (_generateProgress != LoadingState.Done && _generateProgress != LoadingState.Start))
            return false;
        _generateProgress++;

        GenerateRequired = _generateProgress switch
        {
            LoadingState.Villages => 40,
            LoadingState.Castle => 1,
            LoadingState.Paths => 100,
            LoadingState.Forests => 100,
            LoadingState.Done => 0,
            _ => 0
        };

        GenerateCurrent = 0;
        GenerateGoal = _generateProgress.ToString();
        return HasFullyGenerated = _generateProgress == LoadingState.Done;
    }

    private float angle;
    private float distance;
    private Vector2 oldPosition;
    private float oldAngle;
    private float angleOffset = 20F;

    private List<Vector2> _oldPositions = new List<Vector2>();
    private bool _backtracking;
    private int _backtrackCount;

    private void GenerateVillages()
    {
        Vector2? position = null;

        if (GenerateCurrent == 0)
        {
            angle = _random.NextSingle() * (float)Math.PI * 2;
            distance = 200F;
            position = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
        }

        if (angle >= 360F)
        {
            if (_backtracking)
            {
                _backtrackCount++;
                if (_backtrackCount >= _oldPositions.Count)
                {
                    // fail save in case generation breaks
                    Log.WriteError("Unable to generate 40 villages! Restarting village generation!");
                    GenerateCurrent = 0;
                    position = null;
                    _oldPositions.Clear();
                    _overworld.RemoveAll(m => m is Village);
                    _backtracking = false;
                    oldPosition = Vector2.Zero;
                    angle = 0;
                    distance = 0;
                    angleOffset = 0F;
                    _backtrackCount = 0;
                    return;
                }

                oldPosition = _oldPositions[^_backtrackCount];
            }

            angleOffset = 0F;
            angle = 0F;
            _backtracking = true;
        }

        if (distance == 0)
        {
            distance = _bounds.X * 32 / 4;
        }

        angle += 0.1F;
        Vector2 calculatedPosition =
            oldPosition + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;

        Rectangle calculatedRectangle = new Rectangle(calculatedPosition.ToPoint(), Village.DefaultSize.ToPoint());

        if ( // position is in bounds
            Rectangle.Intersect(Rectangle, calculatedRectangle) == calculatedRectangle &&
            // does is not the same as prior based on minimum offset
            angle + angleOffset > oldAngle || angle - angleOffset < oldAngle &&
            // position is not close enough to other villages
            _villagesView.All(v => Vector2.Distance(v.Position, calculatedPosition) >= distance)
           )
        {
            position = calculatedPosition;
            oldPosition = calculatedPosition;
            angleOffset = (float)Math.PI * _random.NextSingle();
            _oldPositions.Add(calculatedPosition);
            _backtrackCount = 0;
            _backtracking = false;
        }


        if (position is null)
            return;

        angle = 0F;
        distance = 0F;

        // ToDo: name from file (add with localization)
        string villageName = GenerateCurrent.ToString();

        var village = new Village((position ?? Vector2.Zero), _random, villageName);
        village.Interacted += () => Interaction?.Invoke(village);
        Log.WriteInformation($"{GenerateCurrent}:{position.ToString()}");
        _overworld.Add(village);
        GenerateCurrent++;
    }

    private void GenerateCastle()
    {
        GenerateCurrent++;
    }

    private void GeneratePaths()
    {
        GenerateCurrent++;
    }

    private void GenerateForests()
    {
        GenerateCurrent++;
    }

    public Guid GetCastle()
        => ((Castle)_overworld.First(m => m is Castle)).GetGuid();
}