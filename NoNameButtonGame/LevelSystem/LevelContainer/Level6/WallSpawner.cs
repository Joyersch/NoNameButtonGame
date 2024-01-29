using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Hitboxes.Collision;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class WallSpawner : IManageable, IInteractable, IMouseActions
{
    private readonly Rectangle _area;
    private readonly float _speed;
    private readonly GlitchBlockCollection _leftWall;
    private readonly GlitchBlockCollection _rightWall;
    private readonly List<GlitchBlockCollection> _projectiles;

    private readonly OverTimeInvoker _timeInvoker;

    public Rectangle Rectangle => _area;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    public WallSpawner(Camera camera, Rectangle area, int wallWidth, double time, float speed)
    {
        _area = area;
        _speed = speed;
        _leftWall = new GlitchBlockCollection(new Vector2(wallWidth, _area.Height));
        _leftWall.ChangeColor(GlitchBlock.Color);
        _leftWall.Enter += o => Enter?.Invoke(o);
        _leftWall.GetCalculator(area)
            .OnX(0F)
            .Move();

        _rightWall = new GlitchBlockCollection(new Vector2(wallWidth, _area.Height));
        _rightWall.ChangeColor(GlitchBlock.Color);
        _rightWall.Enter += o => Enter?.Invoke(o);
        _rightWall.GetCalculator(area)
            .OnX(1F)
            .BySizeX(-1F)
            .Move();

        _timeInvoker = new OverTimeInvoker(time, false);
        _timeInvoker.Trigger += GenerateProjectiles;
        _projectiles = new List<GlitchBlockCollection>();
    }

    public void Start()
    {
        if (_timeInvoker.HasStarted)
            return;

        Log.WriteInformation("Staring sequence");
        _timeInvoker.Start();
    }

    private void GenerateProjectiles()
    {
        GlitchBlockCollection block = new GlitchBlockCollection(GlitchBlock.DefaultSize);
        block.ChangeColor(GlitchBlock.Color);
        block.GetCalculator(_area)
            .OnCenter()
            .Centered()
            .Move();
        block.Enter += o => Enter?.Invoke(o);

        var velocityAdapter = new VelocityAdapter(block);
        velocityAdapter.AddVelocity(new Vector2(0,-1) * _speed);
    }


    public void Update(GameTime gameTime)
    {
        _timeInvoker.Update(gameTime);

        _leftWall.Update(gameTime);
        _rightWall.Update(gameTime);
        foreach (var projectile in _projectiles)
        {
            projectile.Update(gameTime);
        }
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _leftWall.UpdateInteraction(gameTime, toCheck);
        _rightWall.UpdateInteraction(gameTime, toCheck);
        foreach (var projectile in _projectiles)
        {
            projectile.UpdateInteraction(gameTime, toCheck);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _leftWall.Draw(spriteBatch);
        _rightWall.Draw(spriteBatch);
        foreach (var projectile in _projectiles)
        {
            projectile.Draw(spriteBatch);
        }
    }
}