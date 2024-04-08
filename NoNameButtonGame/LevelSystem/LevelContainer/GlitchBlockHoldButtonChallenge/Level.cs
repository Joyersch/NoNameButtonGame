using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.GlitchBlockHoldButtonChallenge;

internal class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random, float difficulty = 1) : base(display, window, random)
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

        // Increasing the block size so, animations align
        var alignment = (Camera.Rectangle.Size.Y - wallSize.Y * 2) % (GlitchBlock.ImageSize.Y * singleScale);
        wallSize.Y += alignment;
        wallSide = new GlitchBlockCollection(wallSize, singleScale);
        wallSide.Enter += Fail;
        wall.ChangeColor(GlitchBlock.Color);
        wallSide.GetAnchor(wall)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.BottomRight)
            //.SetDistanceY(-alignment)
            .Move();
        AutoManaged.Add(wallSide);
    }
}