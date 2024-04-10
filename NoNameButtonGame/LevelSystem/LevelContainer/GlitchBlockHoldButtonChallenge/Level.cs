using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.GlitchBlockHoldButtonChallenge;

internal class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry,
        float difficulty = 950) : base(display, window, random, effectsRegistry)
    {
        var textComponent = TextProvider.GetText("Levels.GlitchBlockHoldButtonChallenge");

        Name = textComponent.GetValue("Name");

        var cleanDifficulty = (difficulty + 100F) / 1050F;
        if (cleanDifficulty > 1F)
            cleanDifficulty = 1F;

        float singleScale = 4F;
        // in order to allign the blocks correctly the size is increased by the single scale;
        float projectedWidth = 15 * cleanDifficulty;
        float overhead = projectedWidth % singleScale;
        float count = projectedWidth - overhead + singleScale;
        if (overhead > singleScale * 0.5F)
            count += singleScale;

        Log.WriteInformation($"Current width:{count}");

        var width = GlitchBlock.ImageSize.X * count;
        var wallSize = new Vector2(width, Camera.Rectangle.Height);
        var wall = new GlitchBlockCollection(wallSize, singleScale);
        wall.Enter += Fail;
        wall.ChangeColor(GlitchBlock.Color);
        wall.GetCalculator(Camera.Rectangle)
            .Move();
        AutoManaged.Add(wall);

        wall = new GlitchBlockCollection(wallSize, singleScale);
        wall.Enter += Fail;
        wall.ChangeColor(GlitchBlock.Color);
        wall.GetCalculator(Camera.Rectangle)
            .OnX(1F)
            .BySizeX(-1F)
            .Move();
        AutoManaged.Add(wall);

        wallSize = new Vector2(Camera.Rectangle.Width - width * 2, GlitchBlock.ImageSize.Y * count);
        var wallSide = new GlitchBlockCollection(wallSize, singleScale);
        wallSide.Enter += Fail;
        wall.ChangeColor(GlitchBlock.Color);
        wallSide.GetAnchor(wall)
            .SetMainAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopRight)
            .Move();
        AutoManaged.Add(wallSide);
        var oldWallSide = wallSide;

        var wallY = wallSize.Y;
        // Increasing the block size so, animations align
        var alignment = (Camera.Rectangle.Size.Y - wallSize.Y * 2) % (GlitchBlock.ImageSize.Y * singleScale);
        wallSize.Y += alignment;
        wallSide = new GlitchBlockCollection(wallSize, singleScale);
        wallSide.Enter += Fail;
        wall.ChangeColor(GlitchBlock.Color);
        wallSide.GetAnchor(wall)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.BottomRight)
            .Move();

        AutoManaged.Add(wallSide);

        Vector2 availableSize = new Vector2(Camera.Rectangle.Size.X - wall.GetSize().X * 2,
            Camera.Rectangle.Size.Y - wallY - wallSize.Y);
        // space available for the mouse
        var available = new Rectangle(oldWallSide.Rectangle.BottomLeftCorner().ToPoint(), availableSize.ToPoint());

        var button = new Button("Finish");
        var holdButton = new HoldButtonAddon(button, 20000F);
        holdButton.Click += Finish;
        SetButton(available, holdButton, random);
        AutoManaged.Add(holdButton);

        var availableSpots = availableSize.X * availableSize.Y;
        availableSpots *= 0.8F;
        float used = 0F;
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