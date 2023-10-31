using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
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
    private GameObject genShowcase;
    public readonly int VillageCount = 30;

    public bool HasFullyGenerated { get; private set; }
    public bool CastleOnScreen { get; private set; }

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
        Castle = 0,
        Villages = 1,
        Paths = 2,
        Forests = 3,
        Done = 4,
    }


    // village generation
    private float _angle = 0F;
    private Vector2 _offset = Vector2.Zero;
    private Village[] _firstThree = null;
    private Village[] _firstTwelve = null;
    private float[] _angles = null;
    private int _radius;
    // village generation

    // forest generation
    private int _x;
    private int _y;
    private bool _stageOne = true;
    private int _forestsSet;
    private ConnectedGameObject[] _forests;
    // forest generation

    // path generation

    // path generation

    public OverworldCollection(Random random, Camera camera, Vector2 bounds)
    {
        _random = random;
        _camera = camera;
        _bounds = bounds;
        _overworld = new List<IManageable>();
        GenerateGoal = _generateProgress.ToString();
        genShowcase = new GameObject(Vector2.Zero, Village.DefaultSize);
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

        genShowcase.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var obj in _overworld)
            if (obj.Rectangle.Intersects(_camera.Rectangle))
                obj.Draw(spriteBatch);
        genShowcase.Draw(spriteBatch);
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
            case LoadingState.Start:
                GenerateStart();
                break;
            case LoadingState.Castle:
                GenerateCastle();
                break;
            case LoadingState.Villages:
                GenerateVillages();
                break;
            case LoadingState.Paths:
                GeneratePaths();
                break;
            case LoadingState.Forests:
                GenerateForests();
                break;
        }

        if (GenerateCurrent < GenerateRequired &&
            (_generateProgress != LoadingState.Done && _generateProgress != LoadingState.Start))
            return false;
        _generateProgress++;

        GenerateRequired = _generateProgress switch
        {
            LoadingState.Castle => 1,
            LoadingState.Villages => VillageCount,
            LoadingState.Paths => VillageCount,
            LoadingState.Forests => 200,
            LoadingState.Done => 0,
            _ => 0
        };

        GenerateCurrent = 0;
        GenerateGoal = _generateProgress.ToString();
        return HasFullyGenerated = _generateProgress == LoadingState.Done;
    }


    private void GenerateStart()
    {
        // If something requires precalculating // setup do it here!
    }

    private void GenerateVillages()
    {
        _angles ??= new float[VillageCount];
        int origin = 0;

        if (GenerateCurrent == 0)
            _angle = (float)(_random.NextDouble() * Math.PI * 2);
        _radius = GenerateCurrent switch
        {
            0 => 800,
            3 => 1000,
            9 => 1300,
            _ => _radius
        };

        switch (GenerateCurrent)
        {
            case < 3:
                _angle += (float)(1F / 3F * Math.PI * 2);
                break;
            case < 12 and >= 3:
                _firstThree ??= new Village[]
                    { (Village)_overworld[1], (Village)_overworld[2], (Village)_overworld[3] };
                origin = (int)GenerateCurrent % 3;
                _offset = _firstThree[origin].Position;
                _angle = GenerateCurrent switch
                {
                    < 6 => _angles[origin] - MathHelper.ToRadians(70F),
                    < 9 => _angles[origin],
                    _ => _angles[origin] + MathHelper.ToRadians(70F)
                };
                break;
            case < 30 and >= 12:
                _firstTwelve ??= new Village[]
                {
                    (Village)_overworld[1], (Village)_overworld[2], (Village)_overworld[3], (Village)_overworld[4],
                    (Village)_overworld[5], (Village)_overworld[6], (Village)_overworld[7], (Village)_overworld[8],
                    (Village)_overworld[9], (Village)_overworld[10], (Village)_overworld[11], (Village)_overworld[12]
                };
                origin = (int)GenerateCurrent % 9 + 3;
                _offset = _firstTwelve[origin].Position;
                origin = (int)GenerateCurrent % 3;
                _angle = GenerateCurrent switch
                {
                    < 15 /*6 + 9*/ => _angles[origin] - MathHelper.ToRadians(30F),
                    < 24 /*15 + 9*/ => _angles[origin],
                    _ => _angles[origin] + MathHelper.ToRadians(30F)
                };
                break;
            default:
                _offset = Vector2.One * -3000;
                break;
        }

        _angles[GenerateCurrent] = _angle;
        Vector2 position = _offset + new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle)) * _radius;

        // ToDo: name from file (add with localization)
        string villageName = GenerateCurrent.ToString();

        Vector2 grid = position - new Vector2(position.X % 32F, position.Y % 32F);

        var village = new Village(grid, _random, villageName);
        village.Interacted += () => Interaction?.Invoke(village);
        Log.WriteInformation($"{GenerateCurrent}:{grid.ToString()}");
        _overworld.Add(village);
        GenerateCurrent++;
    }


    private void GenerateCastle()
    {
        // ToDo: name from file
        var castle = new Castle(Vector2.Zero, 1F, GenerateCurrent.ToString());
        castle.GetCalculator(Rectangle)
            .OnCenter()
            .Centered()
            .Move();
        castle.Interacted += () => Interaction?.Invoke(castle);
        _overworld.Add(castle);
        GenerateCurrent++;
    }

    private int _villagePointer = -1;
    private List<Vector2> _paths = new();

    private void GeneratePaths()
    {
        int origin;
        Vector2 position1 = Vector2.Zero;
        switch (GenerateCurrent)
        {
            case < 3:
                position1 = ((Castle)_overworld[0]).Position;
                break;
            case < 12 and >= 3:
                origin = (int)GenerateCurrent % 3;
                position1 = _firstThree[origin].Position;
                break;
            case < 30 and >= 12:
                origin = (int)GenerateCurrent % 9 + 3;
                position1 = _firstTwelve[origin].Position;
                break;
        }

        Vector2 position2 = ((Village)_overworld[1 + (int)GenerateCurrent]).Position;

        Vector2 path = position2 - position1;
        Vector2 normalizedPath = Vector2.Normalize(path);
        int pathLength = (int)path.Length() / 32;
        for (int i = 0; i < pathLength; i++)
            _paths.Add(position1 + normalizedPath * 32 * i);

        GenerateCurrent++;
    }

    private void GenerateForests()
    {
        Vector2 toCheck = new Vector2(_x * 32, _y * 32);

        if (_stageOne)
        {
            var topLeft = Rectangle.TopLeftCorner();
            bool stop = false;
            for (int i = 0; i < VillageCount && !stop; i++)
            {
                var village = (Village)_overworld[1 + i];
                if (Vector2.Distance(topLeft + toCheck, village.Position) <= 240F)
                {
                    stop = true;
                    break;
                }

            }

            for (int i = 0; i < _paths.Count; i++)
            {
                if (Vector2.Distance(topLeft + toCheck, _paths[i]) <= 128F)
                {
                    stop = true;
                    break;
                }
            }

            if (!stop)
            {
                var forest = new Forest(topLeft + toCheck, _random.Next(0, 2));
                _overworld.Add(forest);
            }

            _x++;
            if (_x > _bounds.X * 2)
            {
                _x = 0;
                _y++;
            }

            if (_y > _bounds.Y * 2)
                _stageOne = false;
            GenerateCurrent = (int)(_y / (_bounds.Y * 2) * 100);
            return;
        }

        if (_forests is null)
        {
            _forests = new ConnectedGameObject[_overworld.Count - VillageCount - 1];
            for (int i = 0; i < _forests.Length; i++)
            {
                _forests[i] = (ConnectedGameObject)_overworld[VillageCount + 1 + i];
            }
        }

        if (_forests.Length != 0)
        {
            var toSet = _forests[_forestsSet];
            var toSetGridPosition = toSet.Position / 32F;
            List<ConnectedGameObject> limitedForests = new();

            for (int i = 0; i < _forests.Length; i++)
            {

                var forest = _forests[i];
                var gridPosition = forest.Position / 32;
                var offset = toSetGridPosition - gridPosition;

                if (offset.X < -1 || offset.Y < -1 || offset.X > 1 || offset.Y > 1)
                    continue;

                limitedForests.Add(forest);
            }

            toSet.SetTextureLocation(limitedForests);
            _forestsSet++;
            GenerateCurrent = 100 + (int)((float)_forestsSet / (float)_forests.Length * 100);
        }
        else
            GenerateCurrent = 200;
    }

    public Guid GetCastle()
        => ((Castle)_overworld.First(m => m is Castle)).GetGuid();
}