using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.SuperGunLevel;

internal class Level : SampleLevel
{
    private Vector2 _lastCursorPosision;
    private float _baseCallTime;
    private float _baseVelocity;

    private float _boostCallTime;
    private float _boostVelocity;
    private OverTimeInvoker _createShot;

    private List<(Vector2 direction, GlitchBlockCollection shot)> _shots;

    private Text _gun;

    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry, int difficulty = 1) :
        base(display, window, random, effectsRegistry)
    {
        var textComponent = TextProvider.GetText("Levels.SuperGunLevel");
        Name = textComponent.GetValue("Name");

        var cleanDifficulty = (difficulty + 100F) / 1050F;
        if (cleanDifficulty > 1F)
            cleanDifficulty = 1F;

        var flippedDifficulty = 1F - cleanDifficulty;

        _baseCallTime = 225F + 475F * flippedDifficulty;
        _baseVelocity = 1F + 2F * cleanDifficulty;

        _boostCallTime = _baseCallTime / 3;
        _boostVelocity = _baseVelocity * 3;

        _shots = new List<(Vector2 direction, GlitchBlockCollection shot)>();

        _gun = new Text(textComponent.GetValue("Gun"));
        _gun.GetCalculator(Camera.Rectangle)
            .OnX(0.1F)
            .OnY(0.5F)
            .Centered()
            .Move();
        AutoManaged.Add(_gun);

        var button = new Button(textComponent.GetValue("Finish"));
        button.GetCalculator(Camera.Rectangle)
            .OnX(0.75F)
            .OnY(0.5F)
            .Centered()
            .Move();

        var addon = new CounterButtonAddon(button, 6 + (int)Math.Floor(15 * cleanDifficulty));
        addon.Click += Finish;
        AutoManaged.Add(addon);

        button.Click += delegate
        {
            addon.GetCalculator(Camera.Rectangle)
                .OnX(random.Next(1, 9) / 10F)
                .OnY(random.Next(1, 9) / 10F)
                .Centered()
                .Move();
        };

        _createShot = new OverTimeInvoker(_baseCallTime, false);
        _createShot.Start();
        _createShot.Trigger += CreateShot;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        var cursorPosition = Cursor.GetPosition();
        var velocity = _baseVelocity;
        var calltime = _baseCallTime;

        if (_lastCursorPosision != cursorPosition)
        {
            velocity = _boostVelocity;
            calltime = _boostCallTime;
        }

        _createShot.ForceChangeTime(calltime);
        _createShot.Update(gameTime);
        (Vector2 direction, GlitchBlockCollection shot)[] shoots = _shots.ToArray();
        foreach (var shot in shoots)
        {
            shot.shot.Move(shot.shot.GetPosition() + shot.direction * velocity);
            shot.shot.Update(gameTime);
            shot.shot.UpdateInteraction(gameTime, Cursor);
            if (!shot.shot.Rectangle.Intersects(Camera.Rectangle))
                _shots.Remove(shot);
        }

        _lastCursorPosision = cursorPosition;
    }

    protected override void Draw(SpriteBatch spriteBatch)
    {
        (Vector2 direction, GlitchBlockCollection shot)[] shoots = _shots.ToArray();
        base.Draw(spriteBatch);
        foreach (var shot in shoots)
        {
            shot.shot.Draw(spriteBatch);
        }

        Cursor.Draw(spriteBatch);
    }

    private void CreateShot()
    {
        var shot = new GlitchBlockCollection(new Vector2(20, 8), 4);
        shot.GetAnchor(_gun)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .Move();
        shot.Enter += Fail;
        var direction = MoveHelper.GetDirection(shot, Cursor);
        _shots.Add((direction, shot));
    }
}