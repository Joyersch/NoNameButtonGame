using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level5;

internal class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level5");

        Name = textComponent.GetValue("Name");

        GlitchBlockCollection leftWall = new GlitchBlockCollection(new Vector2(64, 80), 2F);
        leftWall.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
        leftWall.ChangeColor(GlitchBlock.Color);
AutoManaged.Add(leftWall);

        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }
}