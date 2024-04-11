using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.GlitchBlockHoldButtonChallenge;

internal class Level : SampleLevel
{
    private List<Row> _rows;
    private List<Column> _columns;

    private bool _isInitiated;

    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry,
        float difficulty = 950) : base(display, window, random, effectsRegistry)
    {
        var textComponent = TextProvider.GetText("Levels.GlitchBlockHoldButtonChallenge");

        Name = textComponent.GetValue("Name");

        var cleanDifficulty = (difficulty + 100F) / 1050F;
        if (cleanDifficulty > 1F)
            cleanDifficulty = 1F;

        _rows = new List<Row>();
        _columns = new List<Column>();

        var baseSize = GlitchBlock.ImageSize * 5;
        var x = Camera.Rectangle.Size.X - baseSize.X;
        var y = Camera.Rectangle.Size.Y - baseSize.Y;

        #region Corners

        var leftUpperCorner = new GlitchBlockCollection(baseSize, 5F);
        leftUpperCorner.GetCalculator(Camera.Rectangle)
            .Move();
        leftUpperCorner.Enter += Fail;
        AutoManaged.Add(leftUpperCorner);

        var corner = new Invisible(new Vector2(x, y));
        corner.GetAnchor(leftUpperCorner)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomRight)
            .SetSubAnchor(AnchorCalculator.Anchor.BottomRight)
            .Move();


        var bottomLeftCorner = new GlitchBlockCollection(baseSize, 5F);
        bottomLeftCorner.GetCalculator(Camera.Rectangle)
            .OnY(1F)
            .BySizeY(-1F)
            .Move();
        bottomLeftCorner.Enter += Fail;
        AutoManaged.Add(bottomLeftCorner);

        var topRightCorner = new GlitchBlockCollection(baseSize, 5F);
        topRightCorner.GetCalculator(Camera.Rectangle)
            .OnX(1F)
            .BySizeX(-1F)
            .Move();
        topRightCorner.Enter += Fail;
        AutoManaged.Add(topRightCorner);

        var bottomRightCorner = new GlitchBlockCollection(baseSize, 5F);
        bottomRightCorner.GetCalculator(Camera.Rectangle)
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
            var wall = new Row(available.Width, new Vector2(x, baseSize.Y), 5F);
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
            var wall = new Column(available.Height, new Vector2(baseSize.X, y), 5F);
            wall.GetAnchor(lastWall)
                .SetMainAnchor(AnchorCalculator.Anchor.TopRight)
                .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
                .Move();
            wall.Enter += Fail;

            _columns.Add(wall);
            AutoManaged.Add(wall);
            lastWall = wall;
        }

        var button = new Button(textComponent.GetValue("Finish"));
        SetButton(available.ExtendFromCenter(0.75F), button, random);
        button.Enter += delegate
        {
            if (_isInitiated)
                return;

            _isInitiated = true;
        };

        var holdButton = new HoldButtonAddon(button, 60000);
        holdButton.Click += Finish;
        AutoManaged.Add(holdButton);
    }

    private void SetButton(Rectangle rectangle, IButton button, Random random)
    {
        float x = random.Next(rectangle.Width - button.Rectangle.Width / 2);
        float y = random.Next(rectangle.Height - button.Rectangle.Height / 2);
        var calculator = button.GetCalculator(rectangle)
            .OnX(x / rectangle.Width)
            .OnY(y / rectangle.Height);
        calculator.Move();
    }
}