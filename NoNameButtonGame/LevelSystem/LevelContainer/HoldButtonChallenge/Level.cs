using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Buttons;
using MonoUtils.Ui.Buttons.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.LevelContainer.HoldButtonChallenge;

internal class Level : SampleLevel
{
    private readonly Random _random;
    private List<Row> _rows;
    private List<Column> _columns;
    private Button _button;

    private bool _isInitiated;
    private bool _firstPlay = true;

    public Level(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> settingsAndSaveManager, float difficulty = 1) : base(scene, random,
        effectsRegistry, settingsAndSaveManager)
    {
        _random = random;
        var textComponent = TextProvider.GetText("Levels.GlitchBlockHoldButtonChallenge");

        Name = textComponent.GetValue("Name");

        DnB3.Play();

        var cleanDifficulty = (difficulty + 100F) / 1050F;
        if (cleanDifficulty > 1F)
            cleanDifficulty = 1F;

        _rows = new List<Row>();
        _columns = new List<Column>();

        var flippedDifficulty = 6F - 5F * cleanDifficulty;

        var overTimeInvokeMovement = new OverTimeInvoker(250F * flippedDifficulty, false);
        overTimeInvokeMovement.Trigger += TriggerMovement;
        AutoManaged.Add(overTimeInvokeMovement);

        var baseSize = GlitchBlock.ImageSize * 10;
        var x = Camera.Rectangle.Size.X - baseSize.X;
        var y = Camera.Rectangle.Size.Y - baseSize.Y;

        #region Corners

        var leftUpperCorner = new GlitchBlockCollection(baseSize, 10F);
        leftUpperCorner.InRectangle(Camera)
            .Move();
        leftUpperCorner.Enter += Fail;
        AutoManaged.Add(leftUpperCorner);

        var corner = new Invisible(new Vector2(x, y));
        corner.GetAnchor(leftUpperCorner)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomRight)
            .SetSubAnchor(AnchorCalculator.Anchor.BottomRight)
            .Move();

        var bottomLeftCorner = new GlitchBlockCollection(baseSize, 10F);
        bottomLeftCorner.InRectangle(Camera)
            .OnY(1F)
            .BySizeY(-1F)
            .Move();
        bottomLeftCorner.Enter += Fail;
        AutoManaged.Add(bottomLeftCorner);

        var topRightCorner = new GlitchBlockCollection(baseSize, 10F);
        topRightCorner.InRectangle(Camera)
            .OnX(1F)
            .BySizeX(-1F)
            .Move();
        topRightCorner.Enter += Fail;
        AutoManaged.Add(topRightCorner);

        var bottomRightCorner = new GlitchBlockCollection(baseSize, 10F);
        bottomRightCorner.InRectangle(Camera)
            .OnX(1F)
            .OnY(1F)
            .BySize(-1F)
            .Move();
        bottomRightCorner.Enter += Fail;
        AutoManaged.Add(bottomRightCorner);

        #endregion // Corners

        var availablePosition = leftUpperCorner.Rectangle.BottomRightCorner().ToPoint();
        var availableSize = (Camera.Rectangle.Size.ToVector2() - baseSize * 2).ToPoint();
        Rectangle available = new Rectangle(availablePosition, availableSize);

        IMoveable lastWall = corner;
        for (int i = 0; i < 7; i++)
        {
            var wall = new Row(available.Width, new Vector2(x, baseSize.Y), 10F, _random);
            wall.GetAnchor(lastWall)
                .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
                .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
                .Move();
            wall.Enter += Fail;

            _rows.Add(wall);
            AutoManaged.Add(wall);
            lastWall = wall;
        }

        lastWall = corner;
        for (int i = 0; i < 14; i++)
        {
            var wall = new Column(available.Height, new Vector2(baseSize.X, y), 10F, _random);
            wall.GetAnchor(lastWall)
                .SetMainAnchor(AnchorCalculator.Anchor.TopRight)
                .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
                .Move();
            wall.Enter += Fail;

            _columns.Add(wall);
            AutoManaged.Add(wall);
            lastWall = wall;
        }

        _button = new Button(textComponent.GetValue("Finish"));
        SetButton(available.ExtendFromCenter(0.75F), _button, random);
        _button.Enter += delegate
        {
            if (_isInitiated)
                return;

            _isInitiated = true;
            Log.Information("Starting movement of blocks!");
            overTimeInvokeMovement.Start();
            DnB4.Play();
        };

        var holdButton = new HoldButtonAddon(_button, 15000 + 5000 * cleanDifficulty);
        holdButton.Click += Finish;
        AutoManaged.Add(holdButton);
    }

    private void SetButton(Rectangle rectangle, IButton button, Random random)
    {
        float x = random.Next(rectangle.Width - button.Rectangle.Width / 2);
        float y = random.Next(rectangle.Height - button.Rectangle.Height / 2);
        var calculator = button.InRectangle(new RectangleWrapper(rectangle))
            .OnX(x / rectangle.Width)
            .OnY(y / rectangle.Height);
        calculator.Move();
    }

    private void TriggerMovement()
    {
        bool useRow = _random.Next(2) == 0;

        if (_firstPlay)
        {
            _firstPlay = false;
            var list = (useRow ? _rows.Select(r => r as ISpatial) : _columns.Select(r => r as ISpatial)).ToList();

            var onButton = list.First(i => i.GetRectangle().Intersects(_button.Rectangle)) as IPlayable;
            onButton?.Play();
            return;
        }

        int index = _random.Next(useRow ? _rows.Count : _columns.Count);

        IPlayable playable = useRow ? _rows[index] : _columns[index];
        playable.Play();
    }
}